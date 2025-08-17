using AutoMapper;
using Moq;
using TaskManagementSystem.Api.Entities;
using TaskManagementSystem.Api.Exceptions;
using TaskManagementSystem.Api.Repositories.Interfaces;
using TaskManagementSystem.Api.Services;
using TaskManagementSystem.Api.Services.Interfaces;
using TaskManagementSystem.Shared.DTOs.ToDoTask;

namespace TaskManagementSystem.Api.Tests.Services;

[TestFixture]
public class TaskServiceTests
{
    private Mock<ITaskRepository> _repo;
    private Mock<IMapper> _mapper;
    private Mock<ITaskNotificationService> _notifier;
    private TaskService _sut;

    [SetUp]
    public void SetUp()
    {
        _repo = new Mock<ITaskRepository>();
        _mapper = new Mock<IMapper>();
        _notifier = new Mock<ITaskNotificationService>();

        _sut = new TaskService(_repo.Object, _mapper.Object, _notifier.Object);
    }

    [Test]
    public async Task GetTaskByIdAsync_WhenTaskExists_ReturnsTask()
    {
        // Arrange
        var id = Guid.NewGuid();
        var task = new ToDoTask { Id = id, Title = "Existing Task", IsCompleted = false };

        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(task);

        // Act
        var result = await _sut.GetTaskByIdAsync(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Title, Is.EqualTo("Existing Task"));
        });
    }

    [Test]
    public async Task GetTaskByIdAsync_WhenTaskDoesNotExist_ReturnsNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ToDoTask?)null);

        // Act
        var result = await _sut.GetTaskByIdAsync(id);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAllTasksAsync_SortsIncompleteTasksBeforeCompletedTasks()
    {
        // Arrange
        var completedTask = new ToDoTask { Id = Guid.NewGuid(), Title = "Completed", IsCompleted = true, DueDate = DateTime.Today };
        var incompleteTask = new ToDoTask { Id = Guid.NewGuid(), Title = "Incomplete", IsCompleted = false, DueDate = DateTime.Today.AddDays(1) };

        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync([completedTask, incompleteTask]);

        // Act
        var result = await _sut.GetAllTasksAsync();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.First().IsCompleted, Is.False);
            Assert.That(result.Last().IsCompleted, Is.True);
        });
        _repo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetAllTasksAsync_SortsTasksByDueDateWithinSameCompletionStatus()
    {
        // Arrange
        var task1 = new ToDoTask { Id = Guid.NewGuid(), Title = "Task 1", IsCompleted = false, DueDate = new DateTime(2025, 1, 15) };
        var task2 = new ToDoTask { Id = Guid.NewGuid(), Title = "Task 2", IsCompleted = false, DueDate = new DateTime(2025, 1, 10) };
        var task3 = new ToDoTask { Id = Guid.NewGuid(), Title = "Task 3", IsCompleted = false, DueDate = new DateTime(2025, 1, 12) };

        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync([task1, task2, task3]);

        // Act
        var result = await _sut.GetAllTasksAsync();

        // Assert
        var orderedIds = result.Select(t => t.Id).ToArray();
        var expectedOrder = new[] { task2.Id, task3.Id, task1.Id };
        Assert.That(orderedIds, Is.EqualTo(expectedOrder));
    }

    [Test]
    public async Task GetAllTasksAsync_WhenNoTasks_ReturnsEmptyList()
    {
        // Arrange
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

        // Act
        var result = await _sut.GetAllTasksAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task AddTaskAsync_WhenValidDto_CreatesTaskAndNotifies()
    {
        // Arrange
        var dto = new ToDoTaskUpsertDto { Title = "New Task", Description = "Task description", DueDate = new DateTime(2025, 2, 1) };
        var mappedTask = new ToDoTask { Title = dto.Title, Description = dto.Description, DueDate = dto.DueDate };
        var savedTask = new ToDoTask { Id = Guid.NewGuid(), Title = dto.Title, Description = dto.Description, DueDate = dto.DueDate };
        var resultDto = new ToDoTaskDto { Id = savedTask.Id, Title = savedTask.Title, Description = savedTask.Description, DueDate = savedTask.DueDate };

        _mapper.Setup(m => m.Map<ToDoTask>(dto)).Returns(mappedTask);
        _repo.Setup(r => r.AddAsync(mappedTask)).ReturnsAsync(savedTask);
        _repo.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
        _mapper.Setup(m => m.Map<ToDoTaskDto>(savedTask)).Returns(resultDto);
        _notifier.Setup(n => n.NotifyTaskCreated(resultDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.AddTaskAsync(dto);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("New Task"));
            Assert.That(result.Description, Is.EqualTo("Task description"));
            Assert.That(result.DueDate, Is.EqualTo(new DateTime(2025, 2, 1)));
        });

        _repo.Verify(r => r.AddAsync(mappedTask), Times.Once);
        _repo.Verify(r => r.SaveAllAsync(), Times.Once);
        _notifier.Verify(n => n.NotifyTaskCreated(It.IsAny<ToDoTaskDto>()), Times.Once);
    }

    [Test]
    public void AddTaskAsync_WhenSaveFails_ThrowsSaveOperationFailedExceptionAndDoesNotNotify()
    {
        // Arrange
        var dto = new ToDoTaskUpsertDto { Title = "New Task", DueDate = DateTime.Today };
        var mappedTask = new ToDoTask { Title = dto.Title, DueDate = dto.DueDate };
        var savedTask = new ToDoTask { Id = Guid.NewGuid(), Title = dto.Title, DueDate = dto.DueDate };

        _mapper.Setup(m => m.Map<ToDoTask>(dto)).Returns(mappedTask);
        _repo.Setup(r => r.AddAsync(mappedTask)).ReturnsAsync(savedTask);
        _repo.Setup(r => r.SaveAllAsync()).ReturnsAsync(false);

        // Act & Assert
        var exception = Assert.ThrowsAsync<SaveOperationFailedException>(() => _sut.AddTaskAsync(dto));
        Assert.That(exception!.Message, Is.EqualTo("Failed to save the operation : save the new task to the database."));

        _notifier.Verify(n => n.NotifyTaskCreated(It.IsAny<ToDoTaskDto>()), Times.Never);
    }

    [Test]
    public async Task UpdateTaskAsync_WhenTaskExists_UpdatesTaskAndNotifies()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTask = new ToDoTask { Id = id, Title = "Old Title", Description = "Old desc", DueDate = DateTime.Today };
        var updateDto = new ToDoTaskUpsertDto { Title = "New Title", Description = "New desc", DueDate = DateTime.Today.AddDays(5) };
        var updatedTaskDto = new ToDoTaskDto { Id = id, Title = updateDto.Title, Description = updateDto.Description, DueDate = updateDto.DueDate };

        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingTask);
        _mapper.Setup(m => m.Map(updateDto, existingTask)).Returns(existingTask);
        _repo.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
        _mapper.Setup(m => m.Map<ToDoTaskDto>(existingTask)).Returns(updatedTaskDto);
        _notifier.Setup(n => n.NotifyTaskUpdated(updatedTaskDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.UpdateTaskAsync(id, updateDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        _repo.Verify(r => r.SaveAllAsync(), Times.Once);
        _notifier.Verify(n => n.NotifyTaskUpdated(It.IsAny<ToDoTaskDto>()), Times.Once);
    }

    [Test]
    public void UpdateTaskAsync_WhenTaskDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new ToDoTaskUpsertDto { Title = "New Title", DueDate = DateTime.Today };

        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ToDoTask?)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateTaskAsync(id, updateDto));
        Assert.That(exception!.Message, Is.EqualTo($"Task with id {id} was not found."));

        _repo.Verify(r => r.SaveAllAsync(), Times.Never);
        _notifier.Verify(n => n.NotifyTaskUpdated(It.IsAny<ToDoTaskDto>()), Times.Never);
    }

    [Test]
    public void UpdateTaskAsync_WhenSaveFails_ThrowsSaveOperationFailedExceptionAndDoesNotNotify()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTask = new ToDoTask { Id = id, Title = "Old Title" };
        var updateDto = new ToDoTaskUpsertDto { Title = "New Title", DueDate = DateTime.Today };

        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingTask);
        _mapper.Setup(m => m.Map(updateDto, existingTask)).Returns(existingTask);
        _repo.Setup(r => r.SaveAllAsync()).ReturnsAsync(false);

        // Act & Assert
        var exception = Assert.ThrowsAsync<SaveOperationFailedException>(() => _sut.UpdateTaskAsync(id, updateDto));
        Assert.That(exception!.Message, Is.EqualTo($"Failed to save the operation : update task with id {id}."));

        _notifier.Verify(n => n.NotifyTaskUpdated(It.IsAny<ToDoTaskDto>()), Times.Never);
    }

    [Test]
    public async Task DeleteTaskAsync_WhenTaskExists_DeletesTaskAndNotifies()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTask = new ToDoTask { Id = id, Title = "Task to delete" };

        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingTask);
        _repo.Setup(r => r.Delete(existingTask));
        _repo.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
        _notifier.Setup(n => n.NotifyTaskDeleted(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.DeleteTaskAsync(id);

        // Assert
        Assert.That(result, Is.True);

        _repo.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repo.Verify(r => r.Delete(existingTask), Times.Once);
        _repo.Verify(r => r.SaveAllAsync(), Times.Once);
        _notifier.Verify(n => n.NotifyTaskDeleted(id), Times.Once);
    }

    [Test]
    public void DeleteTaskAsync_WhenTaskDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ToDoTask?)null);

        // Act & Assert
        var exception = Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteTaskAsync(id));
        Assert.That(exception!.Message, Is.EqualTo($"Task with id {id} was not found."));

        // Verify no deletion or notification occurred
        _repo.Verify(r => r.Delete(It.IsAny<ToDoTask>()), Times.Never);
        _repo.Verify(r => r.SaveAllAsync(), Times.Never);
        _notifier.Verify(n => n.NotifyTaskDeleted(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task DeleteTaskAsync_WhenSaveSucceeds_NotifiesAndReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTask = new ToDoTask { Id = id, Title = "Task to delete" };

        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingTask);
        _repo.Setup(r => r.Delete(existingTask));
        _repo.Setup(r => r.SaveAllAsync()).ReturnsAsync(true);
        _notifier.Setup(n => n.NotifyTaskDeleted(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.DeleteTaskAsync(id);

        // Assert
        Assert.That(result, Is.True);
        _notifier.Verify(n => n.NotifyTaskDeleted(id), Times.Once);
    }

    [Test]
    public void DeleteTaskAsync_WhenSaveFails_ThrowsSaveOperationFailedExceptionAndDoesNotNotify()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingTask = new ToDoTask { Id = id, Title = "Task to delete" };

        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingTask);
        _repo.Setup(r => r.Delete(existingTask));
        _repo.Setup(r => r.SaveAllAsync()).ReturnsAsync(false);

        // Act & Assert
        var exception = Assert.ThrowsAsync<SaveOperationFailedException>(() => _sut.DeleteTaskAsync(id));
        Assert.That(exception!.Message, Is.EqualTo($"Failed to save the operation : delete task with id {id}."));

        _notifier.Verify(n => n.NotifyTaskDeleted(It.IsAny<Guid>()), Times.Never);
    }
}