using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static List<Message> _messages = new List<Message>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult MessageBoard()
        {
            return View(_messages);
        }
        [HttpPost]
        public IActionResult MessageBoard(string name, string content)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(content))
            {
                ModelState.AddModelError("", "Name and content are required.");
                return View(_messages);
            }
            var message = new Message
            {
                Name = name,
                Content = content,
                Timestamp = DateTime.Now
            };
            _messages.Add(message);
            return RedirectToAction("MessageBoard");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(string name, string email, string message)
        {
            // Process the contact form submission
            // You can add your logic here to handle the form data
            // Redirect to a confirmation page or show a success message
            ViewData["Message"] = "Thank you for contacting us, " + name + ". We will get back to you soon.";
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
