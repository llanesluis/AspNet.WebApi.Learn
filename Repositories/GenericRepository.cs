using ASPNET_WebAPI.Data;
using ASPNET_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_WebAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly AppDbContext _dbContext;
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetByIdAsNoTrackingAsync(int id)
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            var addedEntity = _dbContext.Set<T>().Add(entity).Entity;
            await _dbContext.SaveChangesAsync();
            return addedEntity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            T updatedEntity = _dbContext.Set<T>().Update(entity).Entity;
            await _dbContext.SaveChangesAsync();
            return updatedEntity;
        }

        public async Task DeleteAsync(T entity)
        {
           _dbContext.Set<T>().Remove(entity);
           await _dbContext.SaveChangesAsync();
        }

        // * However, if the entity uses auto-generated key values, then the Update method can be used for both cases:
        // https://learn.microsoft.com/en-us/ef/core/saving/disconnected-entities#saving-single-entities
        public async Task<T> UpdateOrCreateAsync(T entity)
        {
            var updatedOrCreatedEntity = _dbContext.Set<T>().Update(entity).Entity;
            await _dbContext.SaveChangesAsync();
            return updatedOrCreatedEntity;

            // If the entity is not using auto-generated keys,
            // then the application must decide whether the entity should be inserted or updated: 
            // return _dbContext.Entry(entity).IsKeySet
            //        ? _dbContext.Set<T>().Update(entity).Entity
            //        : _dbContext.Set<T>().Add(entity).Entity;
        }
    }
}
