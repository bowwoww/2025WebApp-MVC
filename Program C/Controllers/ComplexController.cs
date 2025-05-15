using Microsoft.AspNetCore.Mvc;
using Program_C.Models;

namespace Program_C.Controllers
{
    public class ComplexController : Controller
    {
        //complex binding
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Member member)
        {
            ViewData["Name"] = member.Name;
            ViewData["Address"] = member.Address;
            ViewData["Phone"] = member.Phone;
            ViewData["Gender"] = member.Gender;
            return View();
        }
    }
}
