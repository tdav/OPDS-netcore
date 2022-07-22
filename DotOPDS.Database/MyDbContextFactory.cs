using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotOPDS.Database
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<MyDbContext>();
            var connectionString = "Host=127.0.0.1;Database=books_db;Username=postgres;Password=1;Pooling=true;";
            options.UseNpgsql(connectionString, b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging()
                   .UseSnakeCaseNamingConvention();

            return new MyDbContext(options.Options);
        }
    }
}
