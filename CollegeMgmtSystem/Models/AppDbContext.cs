using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CollegeMgmtSystem.Models
{
    public class AppDbContext:IdentityDbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>()
               .HasMany(d => d.Courses)
               .WithOne(c => c.Department)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading deletes for courses

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading deletes for students

            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Teachers)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading deletes for teachers
        }


    }
}
