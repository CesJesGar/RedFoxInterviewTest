#region

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RedFox.Application.Service.Infrastructure;
using RedFox.Domain.Entities;

#endregion

namespace RedFox.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IAppDbContext
{
    public DbSet<User>    Users      => Set<User>();
    public DbSet<Company> Companies  => Set<Company>();
    public DbSet<Address> Addresses  => Set<Address>();
    public DbSet<Geo>     Geos       => Set<Geo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // UserAddress relation
        modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne()
            .HasForeignKey<User>(u => u.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

        //AddressGeo relation
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Geo)
            .WithOne()
            .HasForeignKey<Address>(a => a.GeoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}