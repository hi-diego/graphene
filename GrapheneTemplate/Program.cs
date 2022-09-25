using GrapheneTemplate.Database;
using Graphene.Database.Interfaces;
using Graphene.Graph;
using Graphene.Graph.Interfaces;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// CORS
//builder.WebHost.ConfigureKestrel(o => o.Listen(System.Net.IPAddress.Parse("0.0.0.0"), 8080));
//builder.Services.AddCors(options =>
//    options.AddPolicy(name: "all", policy => {
//        policy.AllowAnyOrigin();
//        policy.AllowAnyHeader();
//        policy.AllowAnyMethod();
//    })
//);
// Register DatabaseContext Mysql
var connectionString = builder.Configuration.GetConnectionString("mysql");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
// Add DBContext
builder.Services.AddDbContext<GrapheneTemplate.Database.DatabaseContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
// Register Graph Services after your DatabaseContext
Graph.RegisterServices<DatabaseContext>(builder);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
