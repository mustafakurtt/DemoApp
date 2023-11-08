using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.EntityConfigurations;

namespace Persistence.Contexts;

public class PostgresqlDbContext : DbContext
{
    protected IConfiguration Configuration { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public PostgresqlDbContext(DbContextOptions<PostgresqlDbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
    {
        Configuration = configuration;
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
    }


}