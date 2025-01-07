using ASPNET_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_WebAPI.Data
{
    // 2 - Create a DbContext => root / Data / AppDbContext.cs
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        // 3 - Add the database sets here (DbSet<T>)
        public DbSet<User> Users { get; set; }
    }

    // **Note: this is using the "primary constructor" syntax
    // "A primary constructor indicates that these parameters are necessary for any instance of the type." 
    // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/instance-constructors#primary-constructors
}
