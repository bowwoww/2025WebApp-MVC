using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using WebApp;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return Login();
        }
        [HttpGet]
        public IActionResult Login()
        {
            var cookieUsername = Request.Cookies["Username"];
            ViewData["cookieUsername"] = cookieUsername;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, bool? RememberMe)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewData["ErrorMessage"] = "Username and password are required.";
                return View();
            }
            // Here you would typically check the credentials against a database
            if (username == "admin" && password == "password")
            {
                //設定session儲存Username資訊 (用於登入後的使用者識別)
                HttpContext.Session.SetString("Username", username);
                //如果有勾選remember me，則儲存cookie
                if (RememberMe == true)
                {
                    Response.Cookies.Append("Username", username, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(30) // Cookie有效期為30天
                    });
                } else
                {
                    Response.Cookies.Delete("Username");
                }
                // Redirect to a secure area of the application
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid username or password.";
                return View();
            }

            
        }
        public IActionResult Logout()
        {
            //登出後清除Session
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
