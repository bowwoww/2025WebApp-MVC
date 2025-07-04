﻿using Microsoft.AspNetCore.Mvc;
using ViewApp.Models;
using System.Linq;

namespace ViewApp.Controllers
{
    public class LoginController : Controller
    {
        string[] id = { "A01", "A02", "A03", "A04", "A05", "A06", "A07" };
        string[] name = { "瑞豐夜市", "新堀江商圈", "六合夜市", "青年夜市", "花園夜市", "大東夜市", "武聖夜市" };

        string[] address = { "813高雄市左營區裕誠路", "800高雄市新興區玉衡里", "800台灣高雄市新興區六合二路",
                "80652高雄市前鎮區凱旋四路758號", "台南市北區海安路三段533號", "台南市東區林森路一段276號",
                "台南市中西區武聖路69巷42號" };

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Account account)
        {
            if (account.Id == "admin" && account.Password == "1234")
            {
                //登入成功

                return RedirectToAction("Index");
            }
            else
            {
                //登入失敗
                ModelState.AddModelError("", "帳號或密碼錯誤");
            }
            return View();
        }


        public List<NightMarket> GetList()
        {
            //採用List模式可以隨時增加或刪除
            List<NightMarket> list = new List<NightMarket>();
            for (int i = 0; i < id.Length; i++)
            {
                NightMarket nm = new NightMarket();
                nm.Id = id[i];
                nm.Name = name[i];
                nm.Address = address[i];
                list.Add(nm);
            }
            return list;
        }

        [HttpGet]
        public IActionResult Index()
        {


            var list = GetList();


            return View(list);


        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            var list = GetList();
            var item = (from n in list
                        where n.Id == id && n.Name.Contains("夜市")
                        select n).FirstOrDefault();

            //var item = list.FirstOrDefault(n => n.Id == id);
            //LINQ查詢語法
            //var item = list.Where(n => n.Id == id).OrderBy(list => list.Id).FirstOrDefault();

            if (item == null)
            {
                //找不到資料時導回至首頁
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public IActionResult ListIndex(string id)
        {
            var list = GetList();
            var item = list.FirstOrDefault(n => n.Id == id);
            if (item == null)
            {
                //找不到資料時導回至首頁
                return RedirectToAction("Index");
            }
            var vm = new VMNightMarket
            {
                NightMarkets = list,
                SelectedNightMarket = item
            };
            return View(vm);
        }

    }
}
