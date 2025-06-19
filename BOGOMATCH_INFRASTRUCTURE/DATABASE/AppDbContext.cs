using BOGOGMATCH_DOMAIN.MODELS.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace BOGOMATCH_INFRASTRUCTURE.DATABASE
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("BOGO_USERS");
        }
    }
}
