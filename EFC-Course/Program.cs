using EFC_Course.Data;
using EFC_Course.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections;
namespace EFC_Course
{

    class EFC_Course
    {
        public static void Main()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var connectionString = config.GetSection("connectionstring").Value;


            using (var context =new AppDbContext())
            {
                foreach (var wallet in context!.Walletss)
                {
                    Console.WriteLine(wallet);
                }
            }
            Console.ReadKey();
        }
        public static void Main3()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var connectionString = config.GetSection("connectionstring").Value;

            var services = new ServiceCollection();

            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(connectionString));

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            using (var context = serviceProvider.GetService<AppDbContext>())
            {
                foreach (var wallet in context!.Walletss)
                {
                    Console.WriteLine(wallet);
                }
            }
            Console.ReadKey();
        }

        static AppDbContext context;
        public static void Main1()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var connectionString = config.GetSection("connectionstring").Value;

            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            context = serviceProvider.GetRequiredService<AppDbContext>();


            var tasks = new[]
            {
                Task.Factory.StartNew(() => Job1()),
                Task.Factory.StartNew(() => Job2())
            };

            Task.WhenAll(tasks).ContinueWith(t => {
                Console.WriteLine("Job1 & Job2 executed concurrently!");
            });

            Console.ReadKey();
        }

        static async Task Job1()
        {
            var w1 = new Wallet { Holder = "Jasem", Balance = 1000m };

            context.Walletss.Add(w1);

            await context.SaveChangesAsync();
        }

        static async Task Job2()
        {
            var w2 = new Wallet { Holder = "Rema", Balance = 900m };

            context.Walletss.Add(w2);

            await context.SaveChangesAsync();

        }

        public static void Main2()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
         
            var connectionString = config.GetSection("constr").Value;


            using (var context = new AppDbContext())
            {
                RetrieveOneWalletByWalletID(context, 5);
            }
            Console.ReadKey();
        }
        void Main1DI()
        {
            // Create a configuration object to load the "appsettings.json" file
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // Get the connection string from the configuration
            var connectionString = config.GetSection("constr").Value;

            // Create a collection of services (like dependency injection container)
            var services = new ServiceCollection();

            // Register the AppDbContext in the services, configuring it to use SQL Server with the connection string
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            // Build the service provider to resolve dependencies (like DbContext)
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Use the AppDbContext from the service provider
            using (var context = serviceProvider.GetService<AppDbContext>())
            {
                foreach (var wallet in context!.Walletss)
                {
                    Console.WriteLine(wallet);
                }
            }
            Console.ReadKey();
        }



        private static void QueryingData()
        {
            using (var Context = new AppDbContext())
            {
                var results = Context.Walletss.Where(wallet => wallet.Balance >= 5000);

                foreach (var wallet in results)
                    Console.WriteLine(wallet);
            }
        }

        private static void DeleteWalletByWalletID(int ID)
        {
            using (var Context = new AppDbContext())
            {
                // getting the Wallet with id
                var Wallet = Context.Walletss.Single(x => x.WalletID == ID);

                // removing that wallet from the Wallets in the memorry
                Context.Walletss.Remove(Wallet);

                // saving changes. (We Updated the wallets in the database (only rows effected)).
                Context.SaveChanges();
            }
        }

        public static Wallet GetWalletByWalletID(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException("ID must be greater than zero.");
            }

            Wallet wallet = new Wallet();
            using (var Context = new AppDbContext())
            {
                wallet = Context.Walletss.Single(x => x.WalletID == ID);
            }
            // تحقق إذا كان wallet غير موجود
            if (wallet == null)
            {
                throw new InvalidOperationException("Wallet not found.");
            }
            return wallet;
        }

        public static bool InsertData()
        {
            // Create a new Wallet object.
            Wallet wallet = new Wallet
            {
                WalletID = 15,
                Holder = "Fatima",
                Balance = 1000
            };

            // Open a connection to the database.
            using (var dbContext = new AppDbContext())
            {
                // Add the wallet to the database.
                dbContext.Walletss.Add(wallet);

                // Save the changes to the database.
               return (dbContext.SaveChanges() > 0);
            }
        }
        public static Wallet RetrieveOneWalletByWalletID(AppDbContext dbContext, int IDToRetrieve)
        {
                var Wallet = dbContext.Walletss.FirstOrDefault(wallet => wallet.WalletID == IDToRetrieve);

                Console.WriteLine(Wallet);
                return Wallet!;
        }

        public static void RetrieveCollectionOfData()
        {
            // All database operations will be withing this down there block 
            using (var dbContext = new AppDbContext())
            {
                foreach (var wallet in dbContext.Walletss)
                {
                    Console.WriteLine(wallet);
                }
            }
        }

        private static ISession CreateSession()
        {
            // Load configuration settings from appsettings.json
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
         
            var constr = config.GetSection("conectionstring").Value; // Get the database connection string from the configuration
       
            var mapper = new ModelMapper(); // Create a ModelMapper instance to map entity classes to database tables

            // Add all type mappings from the assembly containing the Wallet entity
            mapper.AddMappings(typeof(Wallet).Assembly.ExportedTypes);

            // Compile the class mappings into an HbmMapping object
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
           
            //Console.WriteLine(domainMapping.AsString()); // Optionally, print the generated mapping to the console
    
            var hbConfig = new Configuration();  // Create a new Configuration object to configure NHibernate

            // Set the database integration settings
            hbConfig.DataBaseIntegration(c =>
            {
                
                c.Driver<MicrosoftDataSqlClientDriver>();// Specify the database driver to use
               
                c.Dialect<MsSql2012Dialect>(); // Specify the dialect for the database system
                
                c.ConnectionString = constr;// Set the connection string
              
                c.LogSqlInConsole = true;// Enable logging of SQL statements to the console
               
                c.LogFormattedSql = true;// Format the logged SQL statements for better readability
            });
            
            hbConfig.AddMapping(domainMapping);// Add the mapping to the NHibernate configuration
            
            var sessionFactory = hbConfig.BuildSessionFactory();// Create a new ISessionFactory using the configuration
            
            var session = sessionFactory.OpenSession();// Open a new ISession from the factory

            return session;// Return the session
        }
    }
}
