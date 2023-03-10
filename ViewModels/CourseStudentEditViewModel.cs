using RSWEBPROJET1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBPROJET1.ViewModels
{
    public class CourseStudentEditViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<long> SelectedStudents { get; set; }
        public IEnumerable<SelectListItem> StudentList { get; set; }
        public string ProjectUrl { get; set; }
    }
}
