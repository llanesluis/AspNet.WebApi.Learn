using System.ComponentModel.DataAnnotations;

namespace ASPNET_WebAPI.Models
{
    // 1 - Create a Model => root / Models / User.cs
    public class User
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; } 

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
