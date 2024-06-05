using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeMgmtSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace CollegeMgmtSystem.Controllers
{
    [Authorize(Roles = "Admin")]

    public class TeachersController : Controller
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Teachers.Include(t => t.Course).Include(t => t.Department).Include(t => t.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Course)
                .Include(t => t.Department)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeptName");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeacherName,DepartmentId,CourseId,UserId")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", teacher.CourseId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", teacher.DepartmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", teacher.UserId);
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", teacher.CourseId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeptName", teacher.DepartmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", teacher.UserId);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeacherName,DepartmentId,CourseId,UserId")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(teacher).State = EntityState.Detached;

                    var existingTeacher = await _context.Teachers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

                    if (existingTeacher == null)
                    {
                        return NotFound();
                    }

                    // Copy the UserId from the existing student to the updated student
                    teacher.UserId = existingTeacher.UserId;

                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", teacher.CourseId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", teacher.DepartmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", teacher.UserId);
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Course)
                .Include(t => t.Department)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teachers == null)
            {
                return Problem("Entity set 'AppDbContext.Teachers'  is null.");
            }
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
          return (_context.Teachers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult GetCoursesByDepartment(int departmentId)
        {
            var courses = _context.Courses.Where(c => c.DepartmentId == departmentId).ToList();
            return Json(courses);
        }
    }
}
