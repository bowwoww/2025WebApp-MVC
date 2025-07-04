using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyModel_CodeFirst.Models;

namespace MyModel_CodeFirst.ViewControllers
{
    public class VCReply : ViewComponent
    {
        private readonly MessageBoardDBContext _context;

        public VCReply(MessageBoardDBContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            // 取得回覆資訊
            var replies = await _context.Responses
                .Where(r => r.Id == id)
                .OrderByDescending(r => r.SentDate)
                .ToListAsync();
            
            // 傳遞回覆資料到視圖
            return View(replies);
        }
    }
}
