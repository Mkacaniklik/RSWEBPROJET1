//using RSWEB1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RSWEBPROJET1.Areas.Identity.Data;
//using RSWEBPROJET1.Data;
using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBPROJET1.Models { 
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<RSWEBPROJET1User>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            RSWEBPROJET1User user = await UserManager.FindByEmailAsync("admin@rsweb.com");
            if (user == null)
            {
                var User = new RSWEBPROJET1User();
                User.Email = "admin@rsweb.com";
                User.UserName = "admin@rsweb.com";
                string userPWD = "ADMIN111";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            var roleCheck1 = await RoleManager.RoleExistsAsync("Professor");
            if (!roleCheck1) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Professor")); }
            RSWEBPROJET1User user1 = await UserManager.FindByEmailAsync("professor@rsweb.com");
            if (user1 == null)
            {
                var User = new RSWEBPROJET1User();
                User.Email = "professor@rsweb.com";
                User.UserName = "professor@rsweb.com";
                string userPWD = "PROFESSOR222";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Professor"); }
            }

            var roleCheck2 = await RoleManager.RoleExistsAsync("Student");
            if (!roleCheck2) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Student")); }
            RSWEBPROJET1User user2 = await UserManager.FindByEmailAsync("student@rsweb.com");
            if (user2 == null)
            {
                var User = new RSWEBPROJET1User();
                User.Email = "student@rsweb.com";
                User.UserName = "student@rsweb.com";
                string userPWD = "STUDENT333";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Student"); }
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RSWEBPROJET1Context(
            serviceProvider.GetRequiredService<
            DbContextOptions<RSWEBPROJET1Context>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Course.Any() || context.Student.Any() || context.Teacher.Any())
                {
                    return; // DB has been seeded
                };

                context.Teacher.AddRange(
                new Teacher { /*Id = 1, */FirstName = "Simona", LastName = "Naumoska", Degree = "FINKI", AcademicRank = "Docent" },
                new Teacher { /*Id = 2, */FirstName = "Dejan", LastName = "Vasilevski", Degree = "FEIT", AcademicRank = "Doktor" },
                new Teacher { /*Id = 3, */FirstName = "Irina", LastName = "Smith", Degree = "PMF", AcademicRank = "Magister" }
                );
                context.SaveChanges();
                context.Student.AddRange(
          new Student { /*Id = 1, */FirstName = "Sara", LastName = "Trajkovska", AcquiredCredits = 50 },
          new Student { /*Id = 2, */FirstName = "Matej", LastName = "Miloshevski", AcquiredCredits = 45 },
          new Student { /*Id = 3, */FirstName = "Lina", LastName = "Hristovska", AcquiredCredits = 35 },
          new Student { /*Id = 4, */FirstName = "Luka", LastName = "Fasolino", AcquiredCredits = 50 },
          new Student { /*Id = 5, */FirstName = "Elena", LastName = "Angelovska", AcquiredCredits = 45 },
          new Student { /*Id = 6, */FirstName = "Zaklina", LastName = "Kristinovska", AcquiredCredits = 35 },
          new Student { /*Id = 7, */FirstName = "Stefan", LastName = "Petrovikj", AcquiredCredits = 55 },
          new Student { /*Id = 8, */FirstName = "Nikola", LastName = "Dimitrievski", AcquiredCredits = 50 }
          );
                context.SaveChanges();

                context.Course.AddRange(
                new Course
                {
       //Id = 1,
       Title = "Programiranje i algoritmi ",
                    Credits = 6,
                    Semester = 3,
                    Programme = "KTI",
                    FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Simona" && d.LastName == "Naumoska").TeacherId,
                    SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Dejan" && d.LastName == "Vasilevski").TeacherId
                },
             new Course
             {
    //Id = 2,
    Title = "Fizika 1",
                 Credits = 8,
                 Semester = 1,
                 Programme = "KSIAR",
                 FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Irina" && d.LastName == "Smith").TeacherId,
                 SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Dejan" && d.LastName == "Vasilevski").TeacherId
             },
             new Course
             {
    //Id = 3,
    Title = "Kompjuterski Tehnologii",
                 Credits = 4,
                 Semester = 5,
                 Programme = "KTI",
                 FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Simona" && d.LastName == "Naumoska").TeacherId,
                 SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Irina" && d.LastName == "Smith").TeacherId
             }
             );
                context.SaveChanges();

                context.Enrollment.AddRange(
                      new Enrollment { StudentId = 1, CourseId = 1 },
                new Enrollment { StudentId = 2, CourseId = 2 },
                new Enrollment { StudentId = 3, CourseId = 3 },
                new Enrollment { StudentId = 4, CourseId = 1 },
                new Enrollment { StudentId = 5, CourseId = 2 },
                new Enrollment { StudentId = 6, CourseId = 3 },
                new Enrollment { StudentId = 4, CourseId = 1 },
                new Enrollment { StudentId = 5, CourseId = 2 },
                new Enrollment { StudentId = 6, CourseId = 3 }
                         /* new Enrollment
                          {
                              StudentId = context.Student.Single(d => d.FirstName == "Sara" && d.LastName == "Trajkovska").StudentId,
                              CourseId = context.Course.Single(d => d.Title == "Programiranje i algoritmi ").CourseId
                          },
                           new Enrollment
                           {
                               StudentId = context.Student.Single(d => d.FirstName == "Zaklina" && d.LastName == "Kristinovska").StudentId,
                               CourseId = context.Course.Single(d => d.Title == "Kompjuterski Tehnologii").CourseId
                           },
                            new Enrollment
                            {
                                StudentId = context.Student.Single(d => d.FirstName == "Matej" && d.LastName == "Miloshevski").StudentId,
                                CourseId = context.Course.Single(d => d.Title == "Fizika 1").CourseId
                            },
                           new Enrollment
                           {
                               StudentId = context.Student.Single(d => d.FirstName == "Nikola" && d.LastName == "Dimitrievski").StudentId,
                               CourseId = context.Course.Single(d => d.Title == "Programiranje i algoritmi ").CourseId,
                           },
                              new Enrollment
                              {
                                  StudentId = context.Student.Single(d => d.FirstName == "Luka" && d.LastName == "Fasolino").StudentId,
                                  CourseId = context.Course.Single(d => d.Title == "Kompjuterski Tehnologii").CourseId,
                              }*/
                         );
               

            }
        }
    }
}