using System;
using HappyCode.NetCoreBoilerplate.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HappyCode.NetCoreBoilerplate.Core
{
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
                entity.HasIndex(e => e.DeptName)
                    .HasName("dept_name")
                    .IsUnique();

                entity.HasIndex(e => e.MangerNo)
                    .HasName("manger_no");

                entity.Property(e => e.DeptNo).ValueGeneratedNever();

                entity.Property(e => e.DeptName).IsUnicode(false);

                entity.HasOne(d => d.Manger)
                    .WithMany(p => p.LeadingDepartments)
                    .HasForeignKey(d => d.MangerNo)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("departments_ibfk_1");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.DeptNo)
                    .HasName("dept_no");

                entity.Property(e => e.EmpNo).ValueGeneratedNever();

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DeptNo)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("employees_ibfk_1");
            });
        }
    }
}
