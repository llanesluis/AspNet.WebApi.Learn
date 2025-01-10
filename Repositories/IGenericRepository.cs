namespace ASPNET_WebAPI.Repositories
{
    public interface IGenericRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<T?> GetByIdAsNoTrackingAsync(int id);
        public Task<T> CreateAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task<T> UpdateOrCreateAsync(T entity);
        public Task DeleteAsync(T entity);
    }
}
