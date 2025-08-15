using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using WebApi.Models;

namespace WebApi.Service
{
    public class ThirdApiService
    {
        //private static string linkUrl = "https://data.moa.gov.tw/Service/OpenData/TransService.aspx?UnitId=QcbUEzN6E6DL";
        ////選填的參數必須放前面
        //public async Task<List<Animal>> Get(string? animalKind,int? top=200,int? animalAreaPkid=17)
        //{
        //    string newlinkUrl = $"{linkUrl}&$top={top}";
        //    if (animalAreaPkid.HasValue)
        //    {
        //        newlinkUrl += $"&animal_area_pkid={animalAreaPkid.Value}";
        //    }
        //    if (!string.IsNullOrEmpty(animalKind))
        //    {
        //        newlinkUrl += $"&animal_kind={Uri.EscapeDataString(animalKind)}";
        //    }

        //    using (var httpClient = new HttpClient())
        //    {
        //        try
        //        {
        //            // Set the base address for the HttpClient
        //            httpClient.BaseAddress = new Uri(newlinkUrl);
        //            // Send a GET request to the API
        //            var response = await httpClient.GetAsync("");
        //            // Ensure the request was successful
        //            response.EnsureSuccessStatusCode();
        //            // Read the response content as a string
        //            var jsonResponse = await response.Content.ReadAsStringAsync();
        //            // Deserialize the JSON response into a list of Animal objects
        //            //var animals = System.Text.Json.JsonSerializer.Deserialize<List<Animal>>(jsonResponse);
        //            var animals = JsonConvert.DeserializeObject<List<Animal>>(jsonResponse);
        //            return animals ?? new List<Animal>();
        //        }
        //        catch (HttpRequestException e)
        //        {
        //            // Handle any errors that occurred during the request
        //            Console.WriteLine($"Request error: {e.Message}");
        //            return new List<Animal>();
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle any other exceptions that may occur
        //            Console.WriteLine($"An error occurred: {ex.Message}");
        //            return new List<Animal>();
        //        }
        //    }


        //}

        public async Task<IEnumerable<T>> Get<T>(string url)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Set the base address for the HttpClient
                    httpClient.BaseAddress = new Uri(url);
                    // Send a GET request to the API
                    var response = await httpClient.GetAsync("");
                    // Ensure the request was successful
                    response.EnsureSuccessStatusCode();
                    // Read the response content as a string
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response into a list of Animal objects
                    var collections = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonResponse);
                    return collections ?? new List<T>();
                }
                catch (HttpRequestException e)
                {
                    // Handle any errors that occurred during the request
                    Console.WriteLine($"Request error: {e.Message}");
                    return new List<T>();
                }
                catch (Exception ex)
                {
                    // Handle any other exceptions that may occur
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return new List<T>();
                }
            }
        }
    }
}
