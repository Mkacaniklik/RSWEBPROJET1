using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RSWEBPROJET1.Models;
using RSWEBPROJET1.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace RSWEBPROJET1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly RSWEBPROJET1Context _context;

        public StudentsController(RSWEBPROJET1Context context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string SearchString, string SearchString1)
        {
            IQueryable<Student> students = _context.Student.AsQueryable();
            if (!string.IsNullOrEmpty(SearchString) && (!string.IsNullOrEmpty(SearchString1)))
            {
                students = students.Where(c => c.FirstName.Contains(SearchString)).Where(c => c.LastName.Contains(SearchString1));
            }

            else if (!string.IsNullOrEmpty(SearchString) && (string.IsNullOrEmpty(SearchString1)))
            {
                students = students.Where(c => c.FirstName.Contains(SearchString));
            }

            else if (string.IsNullOrEmpty(SearchString) && (!string.IsNullOrEmpty(SearchString1)))
            {
                students = students.Where(c => c.LastName.Contains(SearchString1));
            }
            students = students.Include(c => c.Courses).ThenInclude(c => c.Course);
            var studentsFilteringVM = new StudentPrebaruvanjeViewModel
            {
                Students = await students.ToListAsync()
            };
            return View(studentsFilteringVM);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(long id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
