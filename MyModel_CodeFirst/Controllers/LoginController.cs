using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyModel_CodeFirst.Models;
using System.Security.Claims;

namespace MyModel_CodeFirst.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly MyModel_CodeFirst.Models.MessageBoardDBContext _db;
        public LoginController(ILogger<LoginController> logger, MyModel_CodeFirst.Models.MessageBoardDBContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUser login)
        {
            var user = _db.LoginUsers
                .FirstOrDefault(u => u.UserName == login.UserName && u.Password == login.Password);
            if (user != null)
            {
                // 登入成功，建立 Claims

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role == 1 ? "Admin" : "Guest")
                };
                // 建立 ClaimsIdentity 和 ClaimsPrincipal
                // 這些物件可以用於身份驗證和授權
                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                // 使用 SignInManager 進行登入
                //await HttpContext.SignInAsync("Login",claimsPrincipal);
                // 登入成功後，根據角色導向不同頁面
                return RedirectToAction("Index",user.ReturnUrl);
            }
            else
            {
                ModelState.AddModelError("", "帳號或密碼錯誤");
            }
            return View();

        }
    }
}
