using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RSWEBPROJET1.Models;

namespace RSWEBPROJET1.ViewModels
{
    public class StudentPrebaruvanjeViewModel
    {
        public IList<Student> Students { get; set; }
        public string SearchString { get; set; }

        public string SearchString1 { get; set; }
    }
}
