//using RSWEB1.ViewModels.CourseStudentEditViewModel;
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

namespace RSWEB1.Controllers
{
    public class CoursesController : Controller
    {
        private readonly RSWEBPROJET1Context _context;

        public CoursesController(RSWEBPROJET1Context context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string SearchString, string SearchString1, int courseSemester)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<int> semesterQuery = _context.Course.OrderBy(c => c.Semester).Select(c => c.Semester).Distinct();
            if (!string.IsNullOrEmpty(SearchString) && (!string.IsNullOrEmpty(SearchString1)))
            {
                courses = courses.Where(c => c.Title.Contains(SearchString)).Where(c => c.Programme.Contains(SearchString1));
            }

            else if (!string.IsNullOrEmpty(SearchString) && (string.IsNullOrEmpty(SearchString1)))
            {
                courses = courses.Where(c => c.Title.Contains(SearchString));
            }

            else if (string.IsNullOrEmpty(SearchString) && (!string.IsNullOrEmpty(SearchString1)))
            {
                courses = courses.Where(c => c.Programme.Contains(SearchString1));
            }

            if (courseSemester.Equals(1) || courseSemester.Equals(2) || courseSemester.Equals(3) || courseSemester.Equals(4) || courseSemester.Equals(5) || courseSemester.Equals(6) || courseSemester.Equals(7) || courseSemester.Equals(8))
            {
                courses = courses.Where(x => x.Semester == courseSemester);
            }

            courses = courses.Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(c => c.Student);



            var courseTitleProgrammeVM = new CourseTitleViewModel
            {
                Semesters = new SelectList(await semesterQuery.ToListAsync()),
                Courses = await courses.ToListAsync(),
            };

            return View(courseTitleProgrammeVM);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(c => c.Student)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "TeacherId", "FullName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "TeacherId", "FullName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "TeacherId", "FirstName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "TeacherId", "FirstName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Course.Where(c => c.CourseId == id).Include(c => c.Students).First();
            if (course == null)
            {
                return NotFound();
            }

            var students = _context.Student.AsEnumerable();
            students = students.OrderBy(s => s.FullName);

            CourseStudentEditViewModel viewmodel = new RSWEBPROJET1.ViewModels.CourseStudentEditViewModel
            {
                Course = course,
                StudentList = new MultiSelectList(students, "StudentId", "FullName"),
                SelectedStudents = course.Students.Select(sa => sa.StudentId),
            };

            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "TeacherId", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "TeacherId", "FullName", course.SecondTeacherId);
            return View(viewmodel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RSWEBPROJET1.ViewModels.CourseStudentEditViewModel viewmodel)
        {
            if (id != viewmodel.Course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Course);
                    await _context.SaveChangesAsync();

                    IEnumerable<long> listStudents = viewmodel.SelectedStudents;
                    IQueryable<Enrollment> toBeRemoved = _context.Enrollment.Where(c => !listStudents.Contains(c.StudentId) && c.CourseId == id);
                    _context.Enrollment.RemoveRange(toBeRemoved);

                    IEnumerable<long> existStudents = _context.Enrollment.Where(c => listStudents.Contains(c.StudentId) && c.CourseId == id).Select(c => c.StudentId);
                    IEnumerable<long> newStudents = listStudents.Where(c => !existStudents.Contains(c));
                    foreach (long StudentId in newStudents)
                        _context.Enrollment.Add(new Enrollment { StudentId = StudentId, CourseId = id });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(viewmodel.Course.CourseId))
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "TeacherId", "FullName", viewmodel.Course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "TeacherId", "FullName", viewmodel.Course.SecondTeacherId);
            return View(viewmodel);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
