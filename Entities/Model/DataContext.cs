using Microsoft.EntityFrameworkCore;

namespace Entities.Model
{
    public class DataContext : DbContext    //Inherit DataContext.cs from DbContext (EntityFrameworkCore)
    {
        public DataContext() {}
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)//Connection to SQL Server
        {
            optionsBuilder.UseSqlServer("server=M;database=OrderApp_DB; integrated security=true;"); 
        }
        public DbSet<Customer> Customers { get; set; }  // Set a table for Customer class
        public DbSet<Order> Orders { get; set; }        // Set a table for Order class
    }
}