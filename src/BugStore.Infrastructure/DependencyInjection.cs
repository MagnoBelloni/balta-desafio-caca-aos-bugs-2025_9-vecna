using BugStore.Domain.Interfaces.CacheRepositories;
using BugStore.Domain.Interfaces.Repositories;
using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.Data.CacheRepositories;
using BugStore.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BugStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderCacheRepository, OrderCacheRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddMemoryCache();

        return services;
    }
}
