// It is the main class that coordinates entity framework functionality for a given data model
// DAL - Data Access Layer

using MvcMovie.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcMovie.DAL
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options){

        }
        #nullable disable
        public DbSet<ProductModel> Products  { get; set; } 


        // this method prevents table name from being plualized eg:- products
        // this is a personal perference method so you can omit or include this method
        
        // protected override void OnModelCreating(DbModelBuilder modelBuilder)
        // {
        //     modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        // }
    }
}
