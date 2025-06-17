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
    public class tStudents2Controller : Controller
    {
        //private readonly MyDbContext _context;



        //public tStudents2Controller(MyDbContext context)
        //{
        //    _context = context;
        //}

        MyDbContext _context = new MyDbContext();

        // GET: tStudents2
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.tStudent.Include(t => t.Department);
            return View(await myDbContext.ToListAsync());
        }

        // GET: tStudents2/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tStudent = await _context.tStudent
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.fStuId == id);
            if (tStudent == null)
            {
                return NotFound();
            }

            return View(tStudent);
        }

        // GET: tStudents2/Create
        public IActionResult Create()
        {
            ViewData["departID"] = new SelectList(_context.Department, "DepartID", "DepartID");
            return View();
        }

        // POST: tStudents2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("fStuId,fName,fEmail,fScore,departID")] tStudent tStudent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["departID"] = new SelectList(_context.Department, "DepartID", "DepartID", tStudent.departID);
            return View(tStudent);
        }

        // GET: tStudents2/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tStudent = await _context.tStudent.FindAsync(id);
            if (tStudent == null)
            {
                return NotFound();
            }
            ViewData["departID"] = new SelectList(_context.Department, "DepartID", "DepartID", tStudent.departID);
            return View(tStudent);
        }

        // POST: tStudents2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("fStuId,fName,fEmail,fScore,departID")] tStudent tStudent)
        {
            if (id != tStudent.fStuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tStudentExists(tStudent.fStuId))
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
            ViewData["departID"] = new SelectList(_context.Department, "DepartID", "DepartID", tStudent.departID);
            return View(tStudent);
        }

        // GET: tStudents2/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tStudent = await _context.tStudent
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.fStuId == id);
            if (tStudent == null)
            {
                return NotFound();
            }

            return View(tStudent);
        }

        // POST: tStudents2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tStudent = await _context.tStudent.FindAsync(id);
            if (tStudent != null)
            {
                _context.tStudent.Remove(tStudent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tStudentExists(string id)
        {
            return _context.tStudent.Any(e => e.fStuId == id);
        }
    }
}
