using ASPNET_WebAPI.Data;
using ASPNET_WebAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace ASPNET_WebAPI.Repositories.UserRepository
{
    // 8 - Create a Repository => root / Repositories / ...
    public sealed class UserRepository  : IUserRepository
    {
        // 8.1 - Create private readonly field for the context to be injected
        private readonly AppDbContext _context;

        // 8.2 - Inject the context via the constructor
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // 8.3 - Implement the methods from the interface
        // this layer only works with Entities (takes and returnes Entities, not DTOs)
        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
