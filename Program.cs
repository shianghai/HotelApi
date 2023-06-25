using HotelApi.Data;
using HotelApi.Interfaces;
using HotelApi.Profiles;
using HotelApi.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

//configure logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "c:\\users\\shian\\pers\\HotelApi\\Logs\\log-.txt",
        outputTemplate: "{Timestamp: yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        restrictedToMinimumLevel: LogEventLevel.Information,
        rollingInterval: RollingInterval.Day
    ).CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//use serilog 
builder.Host.UseSerilog();

//add dbcontext
builder.Services.AddDbContext<HotelDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelConnectionString"));
});

//add mapping profile
builder.Services.AddAutoMapper(typeof(MappingProfile));

//add unitofwork
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(
    opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add cors policy
builder.Services.AddCors(c => c.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();
}));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //use cor policy
    app.UseCors("AllowAll");
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
