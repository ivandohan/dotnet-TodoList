using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoList.Basic.Models;

namespace TodoList.AppDataContext 
{
    public class TodoDbContext : DbContext
    {

        private readonly DbSettings _dbSettings;

        public TodoDbContext(IOptions<DbSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }

        public DbSet<TodoModel> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoModel>()
                .ToTable("TodoList")
                .HasKey(t => t.Id);
        }
    }
}