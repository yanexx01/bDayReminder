using Microsoft.EntityFrameworkCore;

namespace Test
{
    class BirthdayContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = bdays.db");
        }
    }
}