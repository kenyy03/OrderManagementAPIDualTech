using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data.Uow;
using OrderManagementAPI.Data.Uow.Enums;

namespace OrderManagementAPI.Data
{
    public static  class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("OrderManagement"));
            });

            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddUnitOfWorkFactory((uow, provider) =>
            {
                uow.RegisterUnitOfWork(UnitOfWorkType.OrderManagementDB, provider.GetRequiredService<ApplicationDbContext>());
            });
            return services;
        }
    }
}
