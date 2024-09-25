using EFC_Course.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace EFC_Course.Data
{
    public class AppDbContext : DbContext
    {


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }





        // This method configures the database connection using the appsettings.json file.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Load the appsettings.json file into a Configuration object.
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // Get the connection string from the configuration.
            var connectionstring = configuration.GetSection("connectionstring").Value;

            // Use the connection string to configure the database connection.
            optionsBuilder.UseSqlServer(connectionstring);
        }

        // This property represents the Wallets table in the database.
        public DbSet<Wallet> Walletss { get; set; } = null!;
        // The 'null!' just tells the compiler not to worry about this starting as null.

        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }


    }
}
