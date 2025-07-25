using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(option =>
    {
        //����json���L�a�ѷ�
        option.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WebApi.Models.GoodStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsConnection")));

// Register the DbContext with a different name for partial class
builder.Services.AddDbContext<WebApi.Models.GoodStoreContext2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsConnection")));

var app = builder.Build();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
