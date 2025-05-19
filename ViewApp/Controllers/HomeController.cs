using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using ViewApp.Models;

namespace MyView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //�ΰ}�C�Ӧs��ժO�W�������
            string[] id = { "A01", "A02", "A03", "A04", "A05", "A06", "A07" };
            string[] name = { "���ש]��", "�sԳ���Ӱ�", "���X�]��", "�C�~�]��", "���]��", "�j�F�]��", "�Z�t�]��" };

            string[] address = { "813����������ϸθ۸�", "800�������s���ϥɿŨ�", "800�x�W�������s���Ϥ��X�G��",
                "80652�������e��ϳͱۥ|��758��", "�x�n���_�Ϯ��w���T�q533��", "�x�n���F�ϪL�˸��@�q276��",
                "�x�n������ϪZ�t��69��42��" };

            //�ĥ�List�Ҧ��i�H�H�ɼW�[�ΧR��
            List<NightMarket> list= new List<NightMarket>();
            //�ĥΰ}�C�Ҧ��N�T�w�}�C�j�p�L�k����ק�
            //NightMarket[] list = new NightMarket[id.Length];

            for (int i = 0; i < id.Length; i++)
            {
                //list�Ҧ�
                NightMarket nm = new NightMarket();
                nm.Id = id[i];
                nm.Name = name[i];
                nm.Address = address[i];
                list.Add(nm);

                //�}�C�Ҧ�
                //list[i] = new NightMarket
                //{
                //    Id = id[i],
                //    Name = name[i],
                //    Address = address[i]
                //};
            }


            return View(list);


        }

        public IActionResult Create()
        {
            return View();
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
