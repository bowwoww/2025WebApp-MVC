using Microsoft.EntityFrameworkCore;

namespace MyModel_CodeFirst.Models
{
    public class SeedData
    {
        //用於初始化資料庫的種子資料
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MessageBoardDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<MessageBoardDBContext>>()))
            {
                // 確保資料庫已建立
                context.Database.EnsureCreated();
                // 檢查是否有資料
                if (context.Messages.Any())
                {
                    return; // 已經有資料了，不需要種子資料
                }
                // 新增一些種子資料
                var ids = new List<string>()
                {
                    Guid.NewGuid().ToString("N"), // 產生新的GUID作為Id
                    Guid.NewGuid().ToString("N")  // 產生另一個新的GUID作為Id
                };

                context.Messages.AddRange(
                    new Message
                    {
                        Id = ids[0],
                        Sender = "Alice",
                        Subject = "Hello World",
                        Body = "This is a test message.",
                        SentDate = DateTime.Now,
                        // 假設上傳的照片檔名為 Id + .jpg
                        UploadPhoto = ids[0] + ".jpg",
                        PhotoType = "jpg"
                    },
                    new Message
                    {
                        Id = ids[1],
                        Sender = "Bob",
                        Subject = "Greetings",
                        Body = "Just saying hi!",
                        SentDate = DateTime.Now.AddMinutes(-10),
                        UploadPhoto = ids[1] + ".jpg",
                        PhotoType = "jpg"
                    }
                );

                context.Responses.AddRange(
                    new Response
                    {
                        Id = ids[0], // 連結到第一條留言
                        Sender = "Charlie",
                        Body = "Nice to meet you!",
                        SentDate = DateTime.Now.AddMinutes(-5)
                    },
                    new Response
                    {
                        Id = ids[1], // 連結到第二條留言
                        Sender = "David",
                        Body = "Thanks for the message!",
                        SentDate = DateTime.Now.AddMinutes(-3)
                    },
                    new Response
                    {
                        Id = ids[0], // 連結到第一條留言
                        Sender = "Eve",
                        Body = "I agree with Charlie!",
                        SentDate = DateTime.Now.AddMinutes(-1)
                    }
                );
                context.SaveChanges();

                string photoPath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot");
                // 假設你有一些預設的照片檔案要放入
                //foreach (var photo in defaultPhotos)
                //{
                //    var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "SeedData", photo);
                //    var destPath = Path.Combine(photoPath, context.Messages.First().Id);
                //    if (!File.Exists(destPath))
                //    {
                //        File.Copy(sourcePath, destPath);
                //    }
                //}
                for (int i =0;i < 2; i++)
                {
                    var sourcePath = Path.Combine(photoPath, "SeedPhotos", $"{i}.jpg");
                    var destPath = Path.Combine(photoPath, "UploadPhotos", $"{ids[i]}.jpg");
                    if (!File.Exists(destPath))
                    {
                        File.Copy(sourcePath, destPath);
                    }
                }
            }


        }
    }
}
