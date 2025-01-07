using ASPNET_WebAPI;
using ASPNET_WebAPI.Data;
using ASPNET_WebAPI.Repositories.UserRepository;
using ASPNET_WebAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => 
{
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status400BadRequest));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// * NOTE: It is possible to add services to the container using an extension method
// you create a static class with a static method that takes IServiceCollection as a parameter
// and returns the services (mutated) after adding the services you want to add (E.g. .AddScoped<I, T>)
builder.Services.AddEndpointsServices();

// 5 - Register the DbContext as a service
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

// 9 - Register the Services as a service
builder.Services.AddScoped<IUserService, UserService>();

// TODO: Document this
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
