using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyModel_CodeFirst.Models;

namespace MyModel_CodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly MessageBoardDBContext db;

        public HomeController(ILogger<HomeController> logger, MessageBoardDBContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            //取四筆最新留言
            var latestMessages = await db.Messages
                .OrderByDescending(m => m.SentDate)
                .Take(4)
                .ToListAsync();

            //foreach (var m in latestMessages)
            //{
            //    m.Responses = await db.Responses
            //        .AsNoTracking()
            //        .Where(r => r.Id == m.Id)
            //        .OrderByDescending(r => r.SentDate)
            //        .ToListAsync();
            //}
            return View(latestMessages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewMessage(string subject, string sender, string body, IFormFile formFile)
        {
            string? uploadPhoto = null;
            string newId = Guid.NewGuid().ToString("N");
            if (formFile != null && formFile.Length > 0)
            {
                if (formFile.ContentType != "image/jpeg" && formFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("formFile", "只允許上傳 JPEG 或 PNG 圖片。");
                    return RedirectToAction("Index");
                }
                var fileName = newId + Path.GetExtension(formFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadPhotos", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                uploadPhoto = fileName;
            }

            if (sender.IsNullOrEmpty() == false)
            {
                Message nm = new Message
                {
                    Subject = subject,
                    Sender = sender,
                    Body = body,
                    SentDate = DateTime.Now,
                    Id = newId,
                };
                if (uploadPhoto != null)
                {
                    nm.UploadPhoto = uploadPhoto;
                }
                db.Messages.Add(nm);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReplyMessage(string subject, string sender, string body)
        {
            if (sender.IsNullOrEmpty() == false)
            {
                Response newResponse = new Response
                {
                    Id = subject,
                    Sender = sender,
                    Body = body,
                    SentDate = DateTime.Now,
                };
                db.Responses.Add(newResponse);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteMessage(string id)
        {
            var message = await db.Messages.FindAsync(id);
            if (message != null)
            {
                db.Messages.Remove(message);
                await db.SaveChangesAsync();
            }
            var reply = await db.Responses.Where(r => r.Id == id).ToListAsync();
            if (reply != null && reply.Count > 0)
            {
                db.Responses.RemoveRange(reply);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
