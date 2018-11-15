using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CompanyXApi.Models;

namespace CompanyXApi.Infrastructure
{
    public partial class CompanyXDBContext : DbContext
    {
        public CompanyXDBContext()
        {
        }

        public CompanyXDBContext(DbContextOptions<CompanyXDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employees> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LEMAK\\SQLEXPRESS;user id=sa;password=root;initial catalog=CompanyXDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.HasIndex(e => e.EmpCode)
                    .IsUnique();

                entity.HasIndex(e => e.EmpFirstName)
                    .HasName("IX_Employees_FirstName");

                entity.HasIndex(e => e.EmpLastName)
                    .HasName("IX_Employees_LastName");

                entity.Property(e => e.EmpId)
                    .HasColumnName("empId")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.EmpCode)
                    .IsRequired()
                    .HasColumnName("empCode")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.EmpFirstName)
                    .HasColumnName("empFirstName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpImage)
                    .HasColumnName("empImage")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.EmpLastName)
                    .IsRequired()
                    .HasColumnName("empLastName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpManager).HasColumnName("empManager");

                entity.Property(e => e.EmpMiddleName)
                    .HasColumnName("empMiddleName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpSalary)
                    .HasColumnName("empSalary")
                    .HasColumnType("decimal(6, 2)");

                entity.Property(e => e.EmpTitle)
                    .IsRequired()
                    .HasColumnName("empTitle")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UctregisteredOn).HasColumnName("UCTRegisteredOn");

                entity.Property(e => e.UctstartDate).HasColumnName("UCTStartDate");

                entity.HasOne(d => d.EmpManagerNavigation)
                    .WithMany(p => p.InverseEmpManagerNavigation)
                    .HasForeignKey(d => d.EmpManager)
                    .HasConstraintName("FK_Employees_Manager");
            });

            modelBuilder.HasSequence("employee_code_seq").HasMin(1);
        }
    }
}
