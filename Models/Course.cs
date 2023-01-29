using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBPROJET1.Models
{
    public class Course
    {
        [Required]
        public int CourseId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public int? Credits { get; set; }

        public int Semester { get; set; }

        [StringLength(100)]
        public string Programme { get; set; } = default!;


        [Display(Name = "Education Level")]
        [StringLength(25)]
        public string EducationLevel { get; set; } = default!;



        public int? FirstTeacherId { get; set; }
        public int? SecondTeacherId { get; set; }

        [Display(Name = "Professor")]
        public Teacher FirstTeacher { get; set; } = default!;


        [Display(Name = "Asisstant")]
        public Teacher SecondTeacher { get; set; } = default!;


        public ICollection<Enrollment> Students { get; set; } = default!;

    }
}