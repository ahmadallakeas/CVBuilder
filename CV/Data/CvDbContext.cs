using CV.Models;
using Microsoft.EntityFrameworkCore;

namespace CV.Data
{
    public class CvDbContext: DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Skills)
                .WithMany(s => s.Users)
                .UsingEntity(j => j.ToTable("UsersSkills"));

        }
        public CvDbContext(DbContextOptions<CvDbContext> options): base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Nationality> Nationality { get; set; }
        public DbSet<Skill> Skill { get; set; }
    }
}
