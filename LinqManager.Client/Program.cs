using LinqManager.Client.Db;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinqManager.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(@"Data Source=DESKTOP-QBP11UB\SQLEXPRESS;Initial Catalog=LinqManagerTestDb;Integrated Security=True")
                    .Options;

            using (var context = new ApplicationDbContext(options)) {
                context.Database.Migrate(); 
            }

        }
    }
}
