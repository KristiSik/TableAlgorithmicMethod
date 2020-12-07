using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TableAlgorithmicMethod.DataAccess.Models;

namespace TableAlgorithmicMethod.DataAccess
{
    public class FileDbContext : DbContext
    {
        public DbSet<Calculation> Statistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=storage.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Calculation>().ToTable("Calculation");
            modelBuilder.Entity<Calculation>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
