using BOGOGMATCH_DOMAIN.MODELS.UserManagement;
using BOGOMATCH_DOMAIN.MODELS.Product;
using Microsoft.EntityFrameworkCore;

namespace BOGOMATCH_INFRASTRUCTURE.DATABASE
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<RatingReview> RatingReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("BOGO_USERS");
            modelBuilder.Entity<RatingReview>()
      .HasOne(r => r.Product)
      .WithMany(p => p.Reviews)
      .HasForeignKey(r => r.ProductId); ;

        }
    }
}
