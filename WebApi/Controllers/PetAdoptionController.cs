using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetAdoptionController : ControllerBase
    {
        private static string linkUrl = "https://data.moa.gov.tw/Service/OpenData/TransService.aspx?UnitId=QcbUEzN6E6DL&$top=200";

        [HttpGet("GetAnimals")]
        public async Task<IEnumerable<Animal>> GetAnimals()
        {
            // data scource 3rd API https://data.moa.gov.tw/Service/OpenData/TransService.aspx?UnitId=QcbUEzN6E6DL
            //using HttpClient to fetch data from the 3rd party API is a common practice.

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Set the base address for the HttpClient
                    httpClient.BaseAddress = new Uri(linkUrl);
                    // Send a GET request to the API
                    var response = await httpClient.GetAsync("");
                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();
                    // Read the response content as a string
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response into a list of Animal objects
                    var animals = System.Text.Json.JsonSerializer.Deserialize<List<Animal>>(jsonResponse);
                    return animals ?? new List<Animal>();
                }
                catch (HttpRequestException e)
                {
                    // Handle any errors that occurred during the request
                    Console.WriteLine($"Request error: {e.Message}");
                    return new List<Animal>();
                }
                catch (Exception ex)
                {
                    // Handle any other exceptions that may occur
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return new List<Animal>();
                }
            }

        }
    }
}
