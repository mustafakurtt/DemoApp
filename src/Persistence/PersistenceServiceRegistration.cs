using Application.Repositories;
using Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MysqlDbContext>(options => options.UseMySQL(configuration.GetConnectionString("Mysql")));
        services.AddDbContext<PostgresqlDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Postgre")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork<IProductRepository>, UnitOfWork<IProductRepository>>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork<ICategoryRepository>, UnitOfWork<ICategoryRepository>>();

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IUnitOfWork<ISupplierRepository>, UnitOfWork<ISupplierRepository>>();
        return services;
    }
}