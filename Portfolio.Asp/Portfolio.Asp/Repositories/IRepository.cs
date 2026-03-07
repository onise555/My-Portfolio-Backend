namespace Portfolio.Asp.Repositories
{
    public interface IRepository<T> where T : class
    {

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
    }
}
