using System.ComponentModel.DataAnnotations;

namespace ASPNET_WebAPI.Models
{
    // 1 - Create a Model => root / Models / User.cs
    public class User : BaseModel
    {
        public required string Name { get; set; } 

        public required string Email { get; set; }

    }
}
