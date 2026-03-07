using Microsoft.EntityFrameworkCore;
using Portfolio.Asp.Data;

namespace Portfolio.Asp.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _data;

        public Repository(DataContext data)
        {
            _data = data;
        }

        public async Task AddAsync(T entity)
        {
            await _data.Set<T>().AddAsync(entity);
            await _data.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _data.Set<T>().Update(entity);
            await _data.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _data.Set<T>().Remove(entity);
            await _data.SaveChangesAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _data.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _data.Set<T>().ToListAsync();
        }
    }
}