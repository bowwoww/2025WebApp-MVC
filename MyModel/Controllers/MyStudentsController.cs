using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyModel.Models;

namespace MyModel.Controllers
{
    public class MyStudentsController : Controller
    {
        MyDbContext db = new MyDbContext();

        public IActionResult Index()
        {
            var students = db.tStudent.Include(s => s.Department).ToList();
            return View(students);
        }

        public IActionResult Create()
        {
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
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public IActionResult Edit(string id)
        {
            var student = db.tStudent.FirstOrDefault(s => s.fStuId == id);
            if (student == null)
            {
                return NotFound();
            }
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
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }
    }
}
