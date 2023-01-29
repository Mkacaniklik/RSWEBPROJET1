using RSWEBPROJET1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBPROJET1.ViewModels
{
    public class TeacherPrebaruvanjeViewModel
    {
        public Teacher Teacher { get; set; }
        public IEnumerable<int> SelectedCourses { get; set; }
        public IEnumerable<SelectListItem> CourseList { get; set; }
        public IList<Teacher> Professor { get; set; }
        public SelectList AcademicRanks { get; set; }
        public string TeacherRank { get; set; }
        public string SearchString { get; set; }
        public string SearchString1 { get; set; }
    }
}
