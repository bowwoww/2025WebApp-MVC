using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetMessage(int skip = 0, int take = 4)
        {
            var messages = await db.Messages
                .OrderByDescending(m => m.SentDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            if(messages.Count == 0)
            {
                return Json("");
            }
            else
            {
                return PartialView("_MessageListPartial", messages);
            }
                
        }

        public async Task<IActionResult> GetReponsesByViewComponent(string id)
        {
            return await Task.Run(() =>
            {
                return ViewComponent("VCReply", new { id = id });
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewMessage(string subject, string sender, string body, IFormFile formFile)
        {
            string? uploadPhoto = null;
            string newId = Guid.NewGuid().ToString("N");
            string fileType = formFile.ContentType;
            if (formFile != null && formFile.Length > 0)
            {
                if (fileType != "image/jpeg" && fileType != "image/png")
                {
                    ModelState.AddModelError("formFile", "只允許上傳 JPEG 或 PNG 圖片。");
                    return Json("ERROR");
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
                    nm.PhotoType = fileType;
                }
                db.Messages.Add(nm);
                await db.SaveChangesAsync();

                return Json(nm);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyMessage(string messageId, string sender, string body)
        {
            if (sender.IsNullOrEmpty() == false)
            {
                Response newResponse = new Response
                {
                    Id = messageId,
                    Sender = sender,
                    Body = body,
                    SentDate = DateTime.Now,
                };
                db.Responses.Add(newResponse);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DeleteMessage(string id)
        {
            var message = await db.Messages.FindAsync(id);
            if (message != null)
            {
                // 刪除上傳的照片檔案
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadPhotos", message.UploadPhoto);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }


                db.Messages.Remove(message);
                await db.SaveChangesAsync();
            }
            //由於關聯資料庫設定 Code First 的 Cascade Delete 通常已將關聯的Fk一併刪除
            var reply = await db.Responses.Where(r => r.Id == id).ToListAsync();
            if (reply != null && reply.Count > 0)
            {
                db.Responses.RemoveRange(reply);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Display(string id)
        {
            var message = await db.Messages
                .Include(m => m.Responses)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
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
