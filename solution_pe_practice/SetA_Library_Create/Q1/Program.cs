using Microsoft.EntityFrameworkCore;
using Q1.Mapping;
using Q1.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PE_Practice_LibraryAContext>(
    otp => otp.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AuthorProfile>());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
