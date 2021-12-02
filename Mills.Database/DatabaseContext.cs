using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Mills.Database.Entities.User;

namespace Mills.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conString = new SqliteConnectionStringBuilder()
            {
                DataSource="Mills.db",
                Cache = SqliteCacheMode.Shared,
                Mode = SqliteOpenMode.ReadWriteCreate,
            }.ToString();


            optionsBuilder.UseSqlite(conString);
        }
}
}