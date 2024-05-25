using HappyCode.NetCoreBoilerplate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyCode.NetCoreBoilerplate.Core
{
    public partial class CarsContext : DbContext
    {
        public CarsContext(DbContextOptions<CarsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.Model).IsUnicode(false);

                entity.Property(e => e.Plate).IsUnicode(false);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Cars_Owners");
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.FullName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);
            });
        }
    }
}
