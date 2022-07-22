using System.Data.Common;
using System.Linq;
using DotOPDS.Database.Models;
using Microsoft.EntityFrameworkCore;
using Toolbelt.ComponentModel.DataAnnotations;

namespace DotOPDS.Database
{
    public partial class MyDbContext : DbContext
    {
        #region DbSet
        public DbSet<Authors> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookTag> BookTags { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
        #endregion

        public MyDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=127.0.0.1;Database=books_db;Username=postgres;Password=1;Pooling=true;")
                             .EnableDetailedErrors()
                             .EnableSensitiveDataLogging()
                             .UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.BuildIndexesFromAnnotations();

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public DbConnection GetDbConnection()
        {
            return Database.GetDbConnection();
        }
    }
}
