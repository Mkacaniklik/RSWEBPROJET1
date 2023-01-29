using RSWEBPROJET1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace RSWEBPROJET1.Models
{
    public class Student
    {
        [Required]
        public long StudentId { get; set; }

        [Display(Name = "First Name")]
        [StringLength(10, MinimumLength = 3)]
        [Required]
        public string FirstName { get; set; } = default!;

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 5)]
        [Required]
        public string LastName { get; set; } = default!;

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        [Display(Name = "Date of enrollment")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Acquired Credits")]
        public int AcquiredCredits { get; set; }

        [Display(Name = "Current Semester")]
        public int CurrentSemester { get; set; }

        [Display(Name = "Education Level")]
        [StringLength(25)]
        public string EducationLevel { get; set; } = default!;

        public ICollection<Enrollment> Courses { get; set; } = default!;
    }
}

