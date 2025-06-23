using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyModel.Models;

namespace MyModel.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly MyDbContext _context;

        public DepartmentsController(MyDbContext context)
        {
            _context = context;
        }
        //MyDbContext _context = new MyDbContext();

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Department.ToListAsync());
        }


        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartID,DepartName")] Department department)
        {
            // 檢查 DepartID 是否重複
            if (DepartmentExists(department.DepartID))
            {
                ModelState.AddModelError("DepartID", "科系代碼已存在，請輸入不同的科系代碼");
            }
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }





        private bool DepartmentExists(string id)
        {
            return _context.Department.Any(e => e.DepartID == id);
        }
    }
}
