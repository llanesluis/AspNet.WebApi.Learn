using ASPNET_WebAPI.DTOs;
using ASPNET_WebAPI.Models;
using ASPNET_WebAPI.Repositories;

namespace ASPNET_WebAPI.Services.UserService
{
    // 10 - Create a Service => root / Services / ... / NameService.cs
    public sealed class UserService : IUserService
    {
        // 10.1 - Create private readonly field for the repository to be injected
        private readonly IGenericRepository<User> _userRepository;

        // 10.2 - Inject the repository via the constructor
        public UserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // 10.3 - Implement the methods from the interface
        // this layer takes (if necessary) and returns filtered data 
        // - to pass data down to the repository layer, data must be converted into Entities
        // - to return data back to the controller, data must be converted into DTOs
        public async Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            
            // Mapping from Entity to DTO
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name
            });
        }

        public async Task<UserDTO?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsNoTrackingAsync(id);

            if (user == null)
                return null;

            // Mapping from Entity to DTO
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };
        }

        public async Task<UserDTO> CreateAsync(CreateUserDTO user, CancellationToken cancellationToken)
        {
            // Mapping from DTO to Entity because the repository expects an Entity
            User userToCreate = new User
            {
                Name = user.Name,
                Email = user.Email
            };

            User createdUser = _userRepository.Create(userToCreate);
            await _userRepository.SaveChangesAsync();

            // Mapping back from Entity to DTO
            return new UserDTO
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                Name = createdUser.Name
            };
        }

        public async Task UpdateAsync(int id, UpdateUserDTO user, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            // To be defined, still don't know how to handle this
            if (existingUser is null)
                throw new Exception("User not found");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            // Decide if the user gets returned to the controller or not
            _userRepository.Update(existingUser);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            // To be defined, still don't know how to handle this
            if (existingUser is null)
                throw new Exception("User not found");

            _userRepository.Delete(existingUser);
            await _userRepository.SaveChangesAsync();
        }
    }
}
