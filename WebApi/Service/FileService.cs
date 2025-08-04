using WebApi.Models;

namespace WebApi.Service
{
    public class FileService
    {
        private readonly GoodStoreContext2 _context;

        public FileService(GoodStoreContext2 context)
        {
            _context = context;
        }

        public async Task deleteFile(string productId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product != null && !string.IsNullOrEmpty(product.Picture))
            {
                var filePath = Path.Combine("wwwroot", "ProductPhotos", product.Picture);
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file: {ex.Message}");
                        throw;
                    }
                }
            }
            // 無需回傳任何值
        }

        public async Task<string> uploadFile(IFormFile file, string productId)
        {
            // 確保上傳檔案為圖片格式 , 否則回傳BadRequest
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            // 檢查檔案副檔名是否在允許的範圍內(轉換為小寫以避免大小寫問題)
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return "";
            }
            // 如要避免因路徑不存在而導致錯誤，可以先檢查並創建目錄
            var directoryPath = Path.Combine("wwwroot", "ProductPhotos");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // 使用 IFormFile 的 FileName 屬性來獲取檔案的副檔名
            var filename = productId + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(directoryPath, filename);
            await deleteFile(productId); // 確保在寫入新檔案前刪除舊檔案
            // using 確保在使用完檔案流後釋放資源,使用 FileStream 來寫入檔案
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filename;
            // 返回檔案名稱以便存儲到資料庫
        }
    }
}
