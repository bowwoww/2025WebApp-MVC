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
            //�ǻ��̷s��1���d����View
            var latestMessages = db.Messages
                .OrderByDescending(m => m.SentDate)
                .Take(4)
                .ToList();
            latestMessages.ForEach(m =>
            {
                //���J�C�ӯd�����^��
                m.Responses = db.Responses
                    .AsNoTracking()
                    .Where(r => r.Id == m.Id)
                    .OrderByDescending(r => r.SentDate)
                    .ToList();
            });
            return View(latestMessages);
        }

        // sumbit�W�[�s��Message

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
                    SentDate = DateTime.Now, //�]�w�o�������{�b�ɶ�
                    Id = Guid.NewGuid().ToString("N"), //���ͷs��GUID�@��Id   
                };
                //�s�W�d��
                db.Messages.Add(nm);
                db.SaveChanges();
            }   
            return RedirectToAction("Index");
        }

        public IActionResult ReplyMessage(string subject,string sender,string body)
        {
            if (sender.IsNullOrEmpty() == false)
            {
                //�s�W�^��
                Response newResponse = new Response
                {
                    Id = subject,
                    Sender = sender,
                    Body = body,
                    SentDate = DateTime.Now, //�]�w�^�Ф�����{�b�ɶ�
                };
                db.Responses.Add(newResponse);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteMessage(string id)
        {
            //�R���d��
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
