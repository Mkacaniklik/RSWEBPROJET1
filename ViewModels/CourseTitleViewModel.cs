using RSWEBPROJET1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBPROJET1.ViewModels
{
    public class CourseTitleViewModel
    {
        public IList<Course> Courses { get; set; }
        public string SearchString { get; set; }
        public string SearchString1 { get; set; }
        public int CourseSemester { get; set; }
        public SelectList Semesters { get; set; }
    }
}

