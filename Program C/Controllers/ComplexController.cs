using Microsoft.AspNetCore.Mvc;
using Program_C.Models;
using System.Linq;

namespace Program_C.Controllers
{
    public class ComplexController : Controller
    {
        private static List<Member> _members = new List<Member>();
        //complex binding

        public IActionResult Index()
        {
            return View(_members);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Member member)
        {
            if(_members.Any(m => m.Id == member.Id))
            {
                ModelState.AddModelError("Id", "ID already exists.");
                return View(member);
            }
            _members.Add(member);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var member = _members.FirstOrDefault(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }
        [HttpPost]
        public IActionResult Edit(Member member)
        {
            var m = _members.FirstOrDefault(m => m.Id == member.Id);
            if (m == null)
            {
                return NotFound();
            }
            m.Name = member.Name;
            m.Address = member.Address;
            m.Phone = member.Phone;
            m.Gender = member.Gender;

            return RedirectToAction("Index");

        }

        public IActionResult Detail(int id)
        {
            var member = _members.FirstOrDefault(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        public IActionResult Delete(int id)
        {
            var member = _members.FirstOrDefault(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }
            _members.Remove(member);
            return RedirectToAction("Index");
        }
    }
}
