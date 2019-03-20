using DatabaseService.Entities;
using System.Data.Entity;

public class ShoematicContext : DbContext
{
    //public DB(string connString)
    //{
    //    this.Database.Connection.ConnectionString = connString;
    //}
    public ShoematicContext() : base("DBService") { }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Product> Products { get; set; }
}