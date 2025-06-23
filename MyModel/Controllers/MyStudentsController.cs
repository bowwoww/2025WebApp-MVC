using System.Collections.Specialized;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyModel.Models;
using MyModel.ViewModel;

namespace MyModel.Controllers
{
    public class MyStudentsController : Controller
    {
        //MyDbContext db = new MyDbContext();

        private readonly MyDbContext db;
        public MyStudentsController(MyDbContext dbContext)
        {
            db = dbContext;
        }
        public IActionResult IndexViewModel(string did = "01")
        {
            // 使用 ViewModel 來傳遞學生資料和部門資料
            var students = db.tStudent.Where(s => s.Department.DepartID == did).ToList();
            var departmentList = db.Department.ToList();
            var viewModel = new VMtStudent
            {
                tStudents = students,
                departments = departmentList
            };
            return View(viewModel);
        }

        public IActionResult Index()
        {
            var students = db.tStudent.Include(s => s.Department).ToList();
            return View(students);
        }

        public IActionResult Create(string did = "01")
        {
            ViewData["depart"] = new SelectList(db.Department, "DepartID", "DepartName", did);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(tStudent student)
        {
            //檢查fStuID 是否重複
            if (db.tStudent.Any(s => s.fStuId == student.fStuId))
            {
                ModelState.AddModelError("fStuId", "學號已存在，請輸入不同的學號");
            }

            if (ModelState.IsValid)
            {
                db.tStudent.Add(student);
                db.SaveChanges();
                return RedirectToAction("IndexViewModel", new { did = student.departID });
            }
            ViewData["depart"] = new SelectList(db.Department, "DepartID", "DepartName", student.departID);
            return View(student);
        }

        public IActionResult Edit(string id)
        {

            var student = db.tStudent.FirstOrDefault(s => s.fStuId == id);
            
            if (student == null)
            {
                return NotFound();
            }
            ViewData["depart"] = new SelectList(db.Department, "DepartID", "DepartName");
            ViewData["departID"] = student.departID;
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id,tStudent student) {

            if(id != student.fStuId)
            {
                ModelState.AddModelError("fStuId", "學號不能更改");
            }

            if (ModelState.IsValid)
            {
                db.tStudent.Update(student);
                db.SaveChanges();
                return RedirectToAction("IndexViewModel");
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            var student = db.tStudent.FirstOrDefault(s => s.fStuId == id);
            if (student == null)
            {
                return NotFound();
            }

            db.tStudent.Remove(student);
            db.SaveChanges();
            return RedirectToAction("IndexViewModel",new {did = student.departID });
        }
    }
}
