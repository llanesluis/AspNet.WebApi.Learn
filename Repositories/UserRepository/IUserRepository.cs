using ASPNET_WebAPI.Models;


namespace ASPNET_WebAPI.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>>GetAllAsync(CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task CreateAsync(User user, CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
        Task DeleteAsync(User user, CancellationToken cancellationToken);
    }
}
