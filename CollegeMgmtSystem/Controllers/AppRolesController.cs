using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CollegeMgmtSystem.Models;

namespace CollegeMgmtSystem.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AppRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly AppDbContext _context;

        public AppRolesController(RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }
            return RedirectToAction("Index");
        }
    }
}
