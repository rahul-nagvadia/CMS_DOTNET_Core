using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeMgmtSystem.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CollegeMgmtSystem.Controllers
{
    [Authorize]
    public class ResultsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ResultsController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Results
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Results.Include(r => r.Course).Include(r => r.Department).Include(r => r.Student);
            return View(await appDbContext.ToListAsync());
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult ShowForm()
        {
            var departments = _context.Departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DeptName
            }).ToList();

            ViewBag.Departments = departments;

            return View(new ResultFormModel());
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]

        public IActionResult ProcessForm(ResultFormModel model)
        {
            int deptId = model.DepartmentId;
            int sem = model.Semester;

            Response.Cookies.Append("SelectedSemester", sem.ToString());
            Response.Cookies.Append("SelectedDepartmentId", deptId.ToString());

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Cms;Trusted_Connection=True");

            using (var dbContext = new AppDbContext(optionsBuilder.Options))
            {

                var students = dbContext.Students
                    .Where(s => s.DepartmentId == deptId && s.Semester == sem)
                    .ToList();

                ViewBag.DepartmentId = deptId;
                ViewBag.Semester = sem;
                return View("StudentList", students);
            }

        }
        [Authorize(Roles = "Admin, Teacher")]

        public IActionResult ShowCourses(int studentId)
        {
            Response.Cookies.Append("SelectedStudentId", studentId.ToString());
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Cms;Trusted_Connection=True");
            using (var dbContext = new AppDbContext(optionsBuilder.Options))
            {
                var student = dbContext.Students.FirstOrDefault(s => s.Id == studentId);
                var courses = dbContext.Courses
                    .Where(c => c.DepartmentId == student.DepartmentId && c.Semester == student.Semester)
                    .ToList();
                var existingResults = dbContext.Results
                    .Where(r =>
                        r.StudentId == studentId &&
                        r.Semester == student.Semester &&
                        r.DepartmentId == student.DepartmentId
                    )
                    .ToList();

                ViewBag.StudentId = studentId;

                // Pass both the courses and existing results to the view
                return View(new Tuple<List<CollegeMgmtSystem.Models.Course>, List<CollegeMgmtSystem.Models.Result>>(courses, existingResults));
            }
        }

        [Authorize(Roles = "Teacher")]

        public IActionResult SaveSessionals(List<CollegeMgmtSystem.Models.Result> Sessionals)
        {
            var selectedSemester = Request.Cookies["SelectedSemester"];
            var selectedDepartmentId = Request.Cookies["SelectedDepartmentId"];
            var selectedStudentId = Request.Cookies["SelectedStudentId"];
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Cms;Trusted_Connection=True");

            using (var dbContext = new AppDbContext(optionsBuilder.Options))
            {
                foreach (var result in Sessionals)
                {
                    result.Semester = int.Parse(selectedSemester!);
                    result.DepartmentId = int.Parse(selectedDepartmentId!);
                    result.StudentId = int.Parse(selectedStudentId!);

                    // Retrieve the existing Result record
                    var existingResult = dbContext.Results.FirstOrDefault(r =>
                        r.Semester == result.Semester &&
                        r.DepartmentId == result.DepartmentId &&
                        r.StudentId == result.StudentId &&
                        r.CourseId == result.CourseId
                    );

                    if (existingResult != null)
                    {
                        // Update the existing Result with the new Sessionals and External marks
                        existingResult.Sessional1 = result.Sessional1;
                        existingResult.Sessional2 = result.Sessional2;
                        existingResult.Sessional3 = result.Sessional3;
                        existingResult.External = result.External;
                    }
                    else
                    {
                        // If no existing record is found, add the new instance to the database
                        dbContext.Results.Add(result);
                    }
                }

                dbContext.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }



        // GET: Results/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Results == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .Include(r => r.Course)
                .Include(r => r.Department)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        public IActionResult ShowIdForm()
        {
            return View();
        }

        public async Task<IActionResult> ProcessIdForm()
        {
            var user = await _userManager.GetUserAsync(User);

            var student = await _context.Students
                .Include(s => s.Department) 
                .FirstOrDefaultAsync(s => s.UserId == user.Id);

            if (student != null)
            {
                var results = await _context.Results
                    .Include(r => r.Course)
                    .Where(r => r.StudentId == student.Id)
                    .ToListAsync();

                if (results != null && results.Any())
                {
                    ViewBag.StudentId = student.Id;
                    return View("ResultDetails", results);
                }
                else
                {
                    ViewBag.Message = "No results found for the student.";
                    return View("ResultError");
                }
                
            }
            else
            {
                    
                return NotFound("Student not found for the logged-in user.");
            }
            
        }

        // GET: Results/Create
        [Authorize(Roles = "Student, Admin")]
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student, Admin")]
        public async Task<IActionResult> Create([Bind("Id,Sessional1,Sessional2,Sessional3,External,Semester,DepartmentId,CourseId,StudentId")] Result result)
        {
            if (ModelState.IsValid)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", result.CourseId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeptName", result.DepartmentId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "StuName", result.StudentId);
            return View(result);
        }

        // GET: Results/Edit/5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Results == null)
            {
                return NotFound();
            }

            var result = await _context.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", result.CourseId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeptName", result.DepartmentId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "StuName", result.StudentId);
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sessional1,Sessional2,Sessional3,External,Semester,DepartmentId,CourseId,StudentId")] Result result)
        {
            if (id != result.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultExists(result.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", result.CourseId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", result.DepartmentId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", result.StudentId);
            return View(result);
        }

        // GET: Results/Delete/5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Results == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .Include(r => r.Course)
                .Include(r => r.Department)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]  
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Results == null)
            {
                return Problem("Entity set 'AppDbContext.Results'  is null.");
            }
            var result = await _context.Results.FindAsync(id);
            if (result != null)
            {
                _context.Results.Remove(result);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultExists(int id)
        {
            return (_context.Results?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}