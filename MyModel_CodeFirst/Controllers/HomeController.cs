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

        // Constructor for dependency injection

        public HomeController(ILogger<HomeController> logger, MessageBoardDBContext db  )
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            //傳遞最新的1筆留言到View
            var latestMessages = db.Messages
                .OrderByDescending(m => m.SentDate)
                .Take(4)
                .ToList();
            latestMessages.ForEach(m =>
            {
                //載入每個留言的回覆
                m.Responses = db.Responses
                    .AsNoTracking()
                    .Where(r => r.Id == m.Id)
                    .OrderByDescending(r => r.SentDate)
                    .ToList();
            });
            return View(latestMessages);
        }

        // sumbit增加新的Message

        [HttpPost]
        public IActionResult NewMessage(string subject,string sender,string body)
        {
            if(sender.IsNullOrEmpty() == false)
            {
                Message nm = new Message
                {
                    Subject = subject,
                    Sender = sender,
                    Body = body,
                    SentDate = DateTime.Now, //設定發文日期為現在時間
                    Id = Guid.NewGuid().ToString("N"), //產生新的GUID作為Id   
                };
                //新增留言
                db.Messages.Add(nm);
                db.SaveChanges();
            }   
            return RedirectToAction("Index");
        }

        public IActionResult ReplyMessage(string subject,string sender,string body)
        {
            if (sender.IsNullOrEmpty() == false)
            {
                //新增回覆
                Response newResponse = new Response
                {
                    Id = subject,
                    Sender = sender,
                    Body = body,
                    SentDate = DateTime.Now, //設定回覆日期為現在時間
                };
                db.Responses.Add(newResponse);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteMessage(string id)
        {
            //刪除留言
            var message = db.Messages.Find(id);
            if (message != null)
            {
                db.Messages.Remove(message);
                db.SaveChanges();
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
