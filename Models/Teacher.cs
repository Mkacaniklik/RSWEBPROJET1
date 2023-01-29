using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBPROJET1.Models
{
    public class Teacher
    {
        [Required]
        public int TeacherId { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string FirstName { get; set; } = default!;

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string LastName { get; set; } = default!;

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        [StringLength(50)]
        public string Degree { get; set; } = default!;

        [Display(Name = "Academic Rank")]
        [StringLength(50)]
        public string AcademicRank { get; set; } = default!;

        [Display(Name = "Office Number")]
        [StringLength(10)]
        public string OfficeNumber { get; set; } = default!;

        [Display(Name = "Date of hiring")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Display(Name = "Courses as Professor")]
        [InverseProperty(nameof(Course.FirstTeacher))]
        public ICollection<Course> CoursesAsFirstTeacher { get; set; } = default!;

        [Display(Name = "Courses as Assistant")]
        [InverseProperty(nameof(Course.SecondTeacher))]
        public ICollection<Course> CoursesAsSecondTeacher { get; set; } = default!;
    }
}
