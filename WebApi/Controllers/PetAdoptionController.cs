using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Models;
using WebApi.Service;
using static System.Net.WebRequestMethods;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetAdoptionController : ControllerBase
    {
        private readonly ThirdApiService _thirdApiService;

        public PetAdoptionController(ThirdApiService thirdApiService)
        {
            _thirdApiService = thirdApiService;
        }


        //[HttpGet]
        //public async Task<IEnumerable<Animal>> GetAnimals(string? urlParameterName, string? urlParameterValue,int? top = 200)
        //{
        //    string url = $"https://data.moa.gov.tw/Service/OpenData/TransService.aspx?UnitId=QcbUEzN6E6DL&{urlParameterName}={urlParameterValue}&$top={top}";
            

        //    return await _thirdApiService.Get<Animal>(url);


        //}

        [HttpGet("GetAnimals")]
        public async Task<IEnumerable<Animal>> GetAnimals(string? urlParameters, int? top = 200)
        {
            // urlParameters 直接是整個string 讓前端可以直接串接多個條件(須注意增加&) 
            string url = $"https://data.moa.gov.tw/Service/OpenData/TransService.aspx?UnitId=QcbUEzN6E6DL{urlParameters}&$top={top}";


            return await _thirdApiService.Get<Animal>(url);


        }

        [HttpGet("GetAnimals/{pageSize}/{page}")]
        public async Task<IEnumerable<Animal>> GetAnimals(string? urlParameters, int pageSize, int page, int? top = 200)
        {
            int skip = (page - 1) * pageSize;

            // urlParameters 直接是整個string 讓前端可以直接串接多個條件(須注意增加&) 
            string url = $"https://data.moa.gov.tw/Service/OpenData/TransService.aspx?UnitId=QcbUEzN6E6DL{urlParameters}&$top={pageSize}&$skip={skip}";


            return await _thirdApiService.Get<Animal>(url);


        }

    }
}
