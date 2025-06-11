using System.Diagnostics.CodeAnalysis;
using HappyCode.NetCoreBoilerplate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyCode.NetCoreBoilerplate.Core
{
    [ExcludeFromCodeCoverage]
    public partial class EmployeesContext : DbContext
    {
        public EmployeesContext(DbContextOptions<EmployeesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Department>(entity =>
            {

                entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

                entity.HasIndex(e => e.MangerId);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DeptName).IsUnicode(false);

                entity.HasOne(d => d.Manger)
                    .WithMany(p => p.LeadingDepartments)
                    .HasForeignKey(d => d.MangerId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("departments_ibfk_1");
            });

            modelBuilder.Entity<Employee>(entity =>
            {

                entity.Property(e => e.Id)
                .HasDefaultValueSql("NEWID()");

                entity.HasIndex(e => e.DeptId);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DeptId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("employees_ibfk_1");
            });
        }
    }
}
