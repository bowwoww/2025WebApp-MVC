using Microsoft.EntityFrameworkCore;

namespace MyModel_CodeFirst.Models
{
    public class MessageBoardDBContext : DbContext
    {
        // 依賴注入
        public MessageBoardDBContext(DbContextOptions<MessageBoardDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        //不能override OnModelCreating方法，否則執行指令建立資料庫時無法生成
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Message>().Metadata.SetIsTableExcludedFromMigrations(true);
        //    modelBuilder.Entity<Response>().Metadata.SetIsTableExcludedFromMigrations(true);
        //}
    }

}
