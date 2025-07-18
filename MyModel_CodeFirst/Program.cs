using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // 全域授權，所有控制器和動作都需要授權
    //options.Filters.Add(new AuthorizeFilter()) 
}
);

builder.Services.AddDbContext<MyModel_CodeFirst.Models.MessageBoardDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MessageBoardConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure authentication
builder.Services.AddAuthentication("Login")
    .AddCookie("Login", options =>
    {
        options.Cookie.HttpOnly = true; // Cookie 只能通過 HTTP 請求訪問，無法通過 JavaScript 訪問
        options.LoginPath = "/Login/Login"; // 登入頁面
        options.LogoutPath = "/Login/Logout"; // 登出頁面
        options.AccessDeniedPath = "/Home/Error"; // 權限不足頁面
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie 有效時間
    });

var app = builder.Build();

// Initialize the database with seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        MyModel_CodeFirst.Models.SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.

// 如果不是開發環境，使用全域異常處理 出現任何未處理的異常時，將會轉到 /Home/Error
if (!app.Environment.IsDevelopment())
{
    // error handling middleware
    app.UseExceptionHandler("/Home/Error");
    // Page not found
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
    // HSTS (HTTP Strict Transport Security) middleware
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication(); // 加這行，啟用驗證
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    // 登入路由 controller : Admin , action : Index

    name: "admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}",
    defaults: new { controller = "Admin", action = "Index" }
    );
app.Run();
