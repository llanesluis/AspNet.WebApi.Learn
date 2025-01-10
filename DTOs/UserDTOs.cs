using System.ComponentModel.DataAnnotations;

namespace ASPNET_WebAPI.DTOs
{
    public class UserDTO
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
    public class CreateUserDTO
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
    public class UpdateUserDTO
    {
        [Required]
        public required int Id { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
