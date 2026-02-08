namespace OrderManagementAPI.Services
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ClienteService>();
            services.AddTransient<ProductoService>();
            services.AddTransient<OrdenService>();
            return services;
        }
    }
}
