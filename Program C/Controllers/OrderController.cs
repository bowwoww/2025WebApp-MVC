using Microsoft.AspNetCore.Mvc;

namespace Program_C.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string inputID,string inputName,string inputPrice)
        {
            ViewData["inputID"] = inputID;
            ViewData["inputName"] = inputName;
            ViewData["inputPrice"] = inputPrice;

            return View();
        }
    }
}
