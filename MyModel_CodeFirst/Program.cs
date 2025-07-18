using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // ������v�A�Ҧ�����M�ʧ@���ݭn���v
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
        options.Cookie.HttpOnly = true; // Cookie �u��q�L HTTP �ШD�X�ݡA�L�k�q�L JavaScript �X��
        options.LoginPath = "/Login/Login"; // �n�J����
        options.LogoutPath = "/Login/Logout"; // �n�X����
        options.AccessDeniedPath = "/Home/Error"; // �v����������
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie ���Įɶ�
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

// �p�G���O�}�o���ҡA�ϥΥ��첧�`�B�z �X�{���󥼳B�z�����`�ɡA�N�|��� /Home/Error
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

app.UseAuthentication(); // �[�o��A�ҥ�����
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    // �n�J���� controller : Admin , action : Index

    name: "admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}",
    defaults: new { controller = "Admin", action = "Index" }
    );
app.Run();
