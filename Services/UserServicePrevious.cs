using ASPNET_WebAPI.Data;
using ASPNET_WebAPI.Models;

namespace ASPNET_WebAPI.Services
{
    // Implement an interface to force the implementation of the methods
    public interface IUserServicePrevious
    {
        public Task<IEnumerable<User>> GetAll();
        public Task<User> GetById(int id);
        public Task<User> Create(User user);
        public void Update(int id, User user);
        public void Delete(int id);
    }

    // 8 - Create a service => root / Services / UserService.cs
    // this service will be injected into the controller
    public class UserServicePrevious : IUserServicePrevious
    {
        // 8.1 - Create a private readonly field for the DbContext to be injected
        private readonly AppDbContext _context;

        // 8.2 - Inject the DbContext via the constructor
        public UserServicePrevious(AppDbContext context)
        {
            _context = context;
        }

        // 8.3 - Implement the methods (required from the interface)
        // Use the methods from the DbContext to interact with the database
        // chain the method .AsNoTracking() to READ ONLY operations to improve performance (GetAll and GetById)

        public async Task<IEnumerable<User>> GetAll()
        {
            // TODO: call the methods from the DbContext to interact with the database

            var user1 = new User { Id = 1, Name = "Fake User 1", Email = "fake.1@gmail.com" };
            var user2 = new User { Id = 2, Name = "Fake User 2", Email = "fake.2@gmail.com" };

            return [user1, user2];
        }

        public async Task<User> GetById(int id)
        {
            // TODO: call the methods from the DbContext to interact with the database

            var users = new User[] {
                new User { Id = 1, Name = "Fake User 1", Email = "fake.1@gmail.com" },
                new User { Id = 2, Name = "Fake User 2", Email = "fake.2@gmail.com" }
            };

            var user = users.FirstOrDefault(u => u.Id == id);

            return user;
        }
        public async Task<User> Create(User user)
        {
            // TODO: call the methods from the DbContext to interact with the database

            var newUser = new User { Id = 999, Name = "Fake User", Email = "fake@gmail.com" };

            return newUser;
        }

        public async void Update(int id, User user)
        {
            // TODO: call the methods from the DbContext to interact with the database
            // - call _context.Entity.Find(id) to find the existing object
            // -mutate the existing object with the new values
            // and then call await _context.SaveChangesAsync(); to persist the changes to the database

            throw new NotImplementedException("Method not implemented yet");
        }

        public async void Delete(int id)
        {
            // TODO: call the methods from the DbContext to interact with the database
            // - call _context.Entity.Find(id) to find the existing object
            // - call _context.Entity.Remove(id) to mark the object for deletion
            // and then call await _context.SaveChangesAsync(); to persist the changes to the database

            throw new NotImplementedException("Method not implemented yet"); 
        }
    }
}
