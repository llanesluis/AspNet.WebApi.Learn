namespace ASPNET_WebAPI
{
    // This file is just a demo file to showcase how to extend the IServiceCollection
    public static class Config
    {
        public static IServiceCollection AddEndpointsServices(this IServiceCollection services)
        {
            // Add endpoint specific services to the container
            // services.AddScoped<>

            return services;
        }
    }
}
