using Demo2.Context;
using Microsoft.EntityFrameworkCore;
using Demo2;
using Demo2.biz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Package>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("PackageDemo"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PackageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseRouting();
}

app.UseHttpsRedirection();

//for error info only in Development environment on appsetting.json
app.UseDeveloperExceptionPage();

app.UseAuthorization();

app.MapControllers();

app.Run();
