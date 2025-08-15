using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Service;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ThirdApiService _thirdApiService;

        public HomeController(ThirdApiService thirdApiService)
        {
            _thirdApiService = thirdApiService;
        }
        public async Task<IActionResult> Index(int pageSize = 30,int page = 1)
        {
            int skip = (page - 1) * pageSize;

            // urlParameters 直接是整個string 讓前端可以直接串接多個條件(須注意增加&) 
            string url = $"https://data.moa.gov.tw/Service/OpenData/TransService.aspx?UnitId=QcbUEzN6E6DL&$top={pageSize}&$skip={skip}";

            var animals = await _thirdApiService.Get<Animal>(url);
            return View(animals.ToList());
        }
    }
}
