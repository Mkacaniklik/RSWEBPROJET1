using RSWEBPROJET1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSWEBPROJET1.ViewModels
{
    public class StudentViewModel
    {
        public List<Student> Students { get; set; }
        public SelectList IDs { get; set; }
        public int studentIndex { get; set; }
    }
}
