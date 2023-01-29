using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RSWEBPROJET1.Areas.Identity.Data;
using RSWEBPROJET1.Models;

namespace RSWEBPROJET1.Models
{
    public class RSWEBPROJET1Context : IdentityDbContext<RSWEBPROJET1User>
    {
        public RSWEBPROJET1Context (DbContextOptions<RSWEBPROJET1Context> options)
            : base(options)
        {
        }

        public DbSet<RSWEBPROJET1.Models.Course> Course { get; set; }

        public DbSet<RSWEBPROJET1.Models.Enrollment> Enrollment { get; set; }

        public DbSet<RSWEBPROJET1.Models.Student> Student { get; set; }

        public DbSet<RSWEBPROJET1.Models.Teacher> Teacher { get; set; }
       protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        /* protected override void OnModelCreating(ModelBuilder builder)
         {
             builder.Entity<Enrollment>()
             .HasOne<Student>(p => p.Student)
             .WithMany(p => p.Courses)
             .HasForeignKey(p => p.StudentId);

             builder.Entity<Enrollment>()
             .HasOne<Course>(p => p.Course)
             .WithMany(p => p.Students)
             .HasForeignKey(p => p.CourseId);

             builder.Entity<Course>()
                 .HasOne<Teacher>(p => p.FirstTeacher)
                 .WithMany(p => p.CoursesAsFirstTeacher)
                 .HasForeignKey(p => p.FirstTeacherId);

             builder.Entity<Course>()
                 .HasOne<Teacher>(p => p.SecondTeacher)
                 .WithMany(p => p.CoursesAsSecondTeacher)
                 .HasForeignKey(p => p.SecondTeacherId);
         }*/
    }
}
