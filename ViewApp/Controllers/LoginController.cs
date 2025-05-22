using Microsoft.AspNetCore.Mvc;
using ViewApp.Models;

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

        [HttpGet]
        public IActionResult Index()
        {

            //採用List模式可以隨時增加或刪除
            List<NightMarket> list = new List<NightMarket>();


            for (int i = 0; i < id.Length; i++)
            {
                //list模式
                NightMarket nm = new NightMarket();
                nm.Id = id[i];
                nm.Name = name[i];
                nm.Address = address[i];
                list.Add(nm);


            }


            return View(list);


        }
    }
}
