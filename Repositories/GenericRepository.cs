using ASPNET_WebAPI.Data;
using ASPNET_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_WebAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> GetByIdAsNoTrackingAsync(int id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        }

        public T Create(T entity)
        {
            var addedEntity = _dbSet.Add(entity).Entity;
            return addedEntity;
        }

        public T Update(T entity)
        {
            T updatedEntity = _dbSet.Update(entity).Entity;
            return updatedEntity;
        }

        public void Delete(T entity)
        {
           _dbSet.Remove(entity);
        }

        // * However, if the entity uses auto-generated key values, then the Update method can be used for both cases:
        // https://learn.microsoft.com/en-us/ef/core/saving/disconnected-entities#saving-single-entities
        public T UpdateOrCreate(T entity)
        {
            var updatedOrCreatedEntity = _dbSet.Update(entity).Entity;
            return updatedOrCreatedEntity;

            // If the entity is not using auto-generated keys,
            // then the application must decide whether the entity should be inserted or updated: 
            // return _dbContext.Entry(entity).IsKeySet
            //        ? _dbSet.Update(entity).Entity
            //        : _dbSet.Add(entity).Entity;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
