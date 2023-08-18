using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OrderWebApi.Model;

namespace OrderWebApi.Context
{
    public class OrderDbContext: DbContext
    {

            public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
            {
                try
                {
                    var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                    if (databaseCreator != null)
                    {
                        //create database if cannot connect
                        if (!databaseCreator.CanConnect()) databaseCreator.Create();

                        //create tables if no tables
                        if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public DbSet<Order> Orders { get; set; }
        
    }
}
