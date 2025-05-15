using Microsoft.AspNetCore.Mvc;

namespace Program_C.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                ViewData["Message"] = "Please select a file to upload.";
                return View();
            }
            if(photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
            {
                ViewData["Message"] = "Only JPEG and PNG files are allowed.";
                return View();
            }
            string fileName = Path.GetFileName(photo.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", fileName);

            //FileStream fileStream = new FileStream(filePath, FileMode.Create);
            //photo.CopyTo(fileStream);

            //使用using語法自動關閉檔案流
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
                ViewData["Message"] = $"File {fileName} uploaded successfully.";
            }

            return View();
        }
    }
}
