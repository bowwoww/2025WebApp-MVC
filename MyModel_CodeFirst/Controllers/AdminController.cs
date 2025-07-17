using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyModel_CodeFirst.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly MyModel_CodeFirst.Models.MessageBoardDBContext _db;
        public AdminController(ILogger<AdminController> logger, MyModel_CodeFirst.Models.MessageBoardDBContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var messages = await _db.Messages
                .OrderByDescending(m => m.SentDate)
                .ToListAsync();
            
            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  
        public async Task<IActionResult> DeleteReply(int id)
        {
            var message = await _db.Responses.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            _db.Responses.Remove(message);
            await _db.SaveChangesAsync();
            return Json(message);
        }

        public async Task<IActionResult> GetResponses(string messageId)
        {
            var responses = await _db.Responses
                .Where(r => r.Id == messageId)
                .OrderByDescending(r => r.SentDate)
                .ToListAsync();

            return ViewComponent("VCReply", new { id = messageId, forAdmin = true });
        }
    }
}
