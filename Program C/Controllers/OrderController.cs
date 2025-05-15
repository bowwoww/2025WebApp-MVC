using Microsoft.AspNetCore.Mvc;
using Program_C.Models;

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

        public IActionResult Create2()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create2(Product product)
        {
            ViewData["inputID"] = product.ID;
            ViewData["inputName"] = product.Name;
            ViewData["inputPrice"] = product.Price;
            return View();
        }
    }
}
