using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(option =>
    {
        //����json���L�a�ѷ�
        option.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WebApi.Models.GoodStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsConnection")));

// Register the DbContext with a different name for partial class
builder.Services.AddDbContext<WebApi.Models.GoodStoreContext2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsConnection")));
// ����FileService �A��
builder.Services.AddScoped<FileService>();

// ����CategoryService �A��
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddScoped<ThirdApiService>();

var app = builder.Build();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllers();

app.Run();

