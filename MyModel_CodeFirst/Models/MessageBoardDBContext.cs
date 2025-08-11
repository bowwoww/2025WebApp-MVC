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

        public virtual DbSet<LoginUser> LoginUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Message>().Metadata.SetIsTableExcludedFromMigrations(true);
            //modelBuilder.Entity<Response>().Metadata.SetIsTableExcludedFromMigrations(true);

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Messages");
                entity.HasKey(e => e.Id).HasName("PK_Message"); // 設定主鍵
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    //.HasDefaultValueSql("newid()"); // 設定主鍵為GUID，並自動生成 (已有C#端程式碼處理)
                    .IsRequired();
                entity.Property(e => e.Sender)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.Subject)
                    .HasMaxLength(100);
                entity.Property(e => e.SentDate)
                    .HasColumnType("datetime"); // 可使用 datetime2 類型以獲得更高的時間精度最高到小數七位
                //    .HasDefaultValueSql("getdate()");  從sql server 取得當前時間  已由 C# 端程式碼處理
                entity.Property(e => e.UploadPhoto)
                    .HasMaxLength(50);
                entity.Property(e => e.PhotoType)
                    .HasMaxLength(20);
                entity.Property(e => e.Body)
                    .IsRequired();
            });

            modelBuilder.Entity<Response>(entity =>
            {
                entity.ToTable("Responses");
                entity.HasKey(e => e.ResponseId).HasName("PK_Response"); // 設定主鍵
                entity.Property(e => e.ResponseId)
                    .ValueGeneratedOnAdd(); // 自動生成
                entity.Property(e => e.Sender)
                    .HasMaxLength(50)
                    .IsRequired();
                entity.Property(e => e.SentDate)
                    .HasColumnType("datetime"); // 可使用 datetime2 類型以獲得更高的時間精度最高到小數七位
                //    .HasDefaultValueSql("getdate()");
                entity.Property(e => e.Body)
                    .IsRequired();
                entity.HasOne(d => d.Message)
                    .WithMany(p => p.Responses)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade); // 設定外鍵關聯
            });

        }
    }

}
