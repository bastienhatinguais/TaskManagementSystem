namespace TaskManagementSystem.Api.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entity);
    void Delete(T entity);

    T Update(T entity);
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    void Attach(T entity);
    void AttachRange(IEnumerable<T> entities);
}