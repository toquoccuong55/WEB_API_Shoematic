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
    public DbSet<Product> Products { get; set; }

    public System.Data.Entity.DbSet<DatabaseService.Entities.Category> Categories { get; set; }

    public System.Data.Entity.DbSet<DatabaseService.Entities.ProductImage> ProductImages { get; set; }

    public System.Data.Entity.DbSet<DatabaseService.Entities.ProductSku> ProductSkus { get; set; }

    public System.Data.Entity.DbSet<DatabaseService.Entities.Order> Orders { get; set; }

    public System.Data.Entity.DbSet<DatabaseService.Entities.OrderDetail> OrderDetails { get; set; }
}