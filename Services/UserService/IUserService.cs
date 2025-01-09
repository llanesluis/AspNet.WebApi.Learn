using ASPNET_WebAPI.DTOs;

namespace ASPNET_WebAPI.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserDTO?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<UserDTO> CreateAsync(CreateUserDTO user, CancellationToken cancellationToken);
        Task UpdateAsync(int id, UpdateUserDTO user, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);


    }
}
