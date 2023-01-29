using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RSWEBPROJET1.Models;
using RSWEBPROJET1.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace RSWEBPROJET1.Controllers
{
    public class TeachersController : Controller
    {
        private readonly RSWEBPROJET1Context _context;

        public TeachersController(RSWEBPROJET1Context context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string SearchString, string SearchString1, string teacherRank)
        {
            IQueryable<Teacher> teachers = _context.Teacher.AsQueryable();
            IQueryable<string> rangQuery = _context.Teacher.OrderBy(c => c.AcademicRank).Select(c => c.AcademicRank).Distinct();

            if (!string.IsNullOrEmpty(SearchString) && (!string.IsNullOrEmpty(SearchString1)))
            {
                teachers = teachers.Where(c => c.FirstName.Contains(SearchString)).Where(c => c.LastName.Contains(SearchString1));
            }

            else if (!string.IsNullOrEmpty(SearchString) && (string.IsNullOrEmpty(SearchString1)))
            {
                teachers = teachers.Where(c => c.FirstName.Contains(SearchString));
            }



            else if (string.IsNullOrEmpty(SearchString) && (!string.IsNullOrEmpty(SearchString1)))
            {
                teachers = teachers.Where(c => c.LastName.Contains(SearchString1));
            }

            if (!string.IsNullOrEmpty(teacherRank))
            {
                teachers = teachers.Where(c => c.AcademicRank == teacherRank);
            }

            teachers = teachers.Include(c => c.CoursesAsFirstTeacher)
                               .Include(c => c.CoursesAsSecondTeacher);



            var teacherfilterVM = new TeacherPrebaruvanjeViewModel
            {
                AcademicRanks = new SelectList(await rangQuery.ToListAsync()),
                Professor = await teachers.ToListAsync()
            };
            return View(teacherfilterVM);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var teacher = await _context.Teacher.Include(c => c.CoursesAsFirstTeacher)
                .Include(c => c.CoursesAsSecondTeacher)
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = _context.Teacher.Where(c => c.TeacherId == id).Include(c => c.CoursesAsFirstTeacher).First();
            if (teacher == null)
            {
                return NotFound();
            }
            var courses = _context.Course.AsEnumerable();
            courses = courses.OrderBy(c => c.Title);

            TeacherPrebaruvanjeViewModel viewmodel = new TeacherPrebaruvanjeViewModel
            {
                Teacher = teacher,
                CourseList = new MultiSelectList(courses, "CourseId", "Title", "Credits"),
                SelectedCourses = teacher.CoursesAsFirstTeacher.Select(c => c.CourseId)
            };


            return View(viewmodel);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeacherPrebaruvanjeViewModel viewmodel)
        {
            if (id != viewmodel.Teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Teacher);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> listCourses = viewmodel.SelectedCourses;
                    IQueryable<Course> toBeRemoved = _context.Course.Where(s => !listCourses.Contains(s.CourseId) && s.FirstTeacherId == id);
                    _context.Course.RemoveRange(toBeRemoved);

                    IEnumerable<int> existCourses = _context.Course.Where(s => listCourses.Contains(s.CourseId) && s.FirstTeacherId == id).Select(s => s.CourseId);
                    IEnumerable<int> newCourses = listCourses.Where(s => !existCourses.Contains(s));
                    foreach (int courseId in newCourses)
                        _context.Course.Add(new Course { FirstTeacherId = id });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(viewmodel.Teacher.TeacherId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewmodel);
        }

        // GET: Teachers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.TeacherId == id);
        }
    }
}
