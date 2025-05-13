using Microsoft.AspNetCore.Mvc;

namespace Program_C.Controllers
{
    public class NewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string Hello()
        {
            return "Hello from NewController!";
        }

        [HttpGet]
        public IActionResult Rainbow()
        {
            string returnValue = "";
            string[] colors = { "Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet" };

            for(int i = 0; i < colors.Length; i++)
            {
                returnValue += $"<span style='color:{colors[i]};'>{colors[i]}</span>";
            }
            returnValue += "<br>";
            for (int i=1;i<9;i++)
            {
                returnValue += $"<a href='/New/ShowPhoto?num={i}'><img src='/images/{i}.jpg' width='200px'></a>";
            }

            //Response.ContentType = "text/html; charset=utf-8";
            //Response.WriteAsync(returnValue);

            return Content(returnValue, "text/html; charset=utf-8");
            //ViewData["photos"] = returnValue;
            //return View();
        }

        public IActionResult ShowPhoto(int num)
        {
            string[] name = { "櫻桃鴨", "鴨油高麗菜", "鴨油麻婆豆腐", "櫻桃鴨握壽司", "片皮鴨捲三星蔥", "三杯鴨", "櫻桃鴨片肉", "慢火白菜燉鴨湯" };
            ViewData["name"] = name[num - 1];
            ViewData["photo"] = $"<img src='/images/{num}.jpg'>";

            return View();
        }
    }
}
