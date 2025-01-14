namespace ASPNET_WebAPI.Repositories
{
    public interface IGenericRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<T?> GetByIdAsNoTrackingAsync(int id);
        public T Create(T entity);
        public T Update(T entity);
        public void Delete(T entity);
        public T UpdateOrCreate(T entity);
        public Task SaveChangesAsync();
    }
}
