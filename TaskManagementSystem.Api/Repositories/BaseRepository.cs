using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Api.Repositories.Interfaces;

namespace TaskManagementSystem.Api.Repositories;

public abstract class BaseRepository<T>(AppDbContext context) : IBaseRepository<T> where T : class
{
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);

    public T Update(T entity) => DbSet.Update(entity).Entity;

    public async Task<List<T>> GetAllAsync() => await DbSet.ToListAsync();

    public async Task<T> AddAsync(T entity) => (await DbSet.AddAsync(entity)).Entity;

    public async Task AddRangeAsync(IEnumerable<T> entity) => await DbSet.AddRangeAsync(entity);

    public void Delete(T entity)
    {
        var entry = DbSet.Local.FirstOrDefault(e => e == entity);
        if (entry == null) DbSet.Attach(entity);
        DbSet.Remove(entity);
    }

    public void Attach(T entity) => DbSet.Attach(entity);

    public void AttachRange(IEnumerable<T> entities) => DbSet.AttachRange(entities);

    public async Task<bool> SaveAllAsync() => await context.SaveChangesAsync() > 0;
}
