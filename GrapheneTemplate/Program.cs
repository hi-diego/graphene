using GrapheneTemplate.Database;
using Microsoft.EntityFrameworkCore;
using Graphene.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Register DatabaseContext Mysql
// var connectionString = builder.Configuration.GetConnectionString("mysql");
// var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
// builder.Services.AddDbContext<GrapheneCache>(
//     dbContextOptions => dbContextOptions
//         .UseMySql(connectionString, serverVersion)
//         .LogTo(Console.WriteLine, LogLevel.Information)
//         .EnableSensitiveDataLogging()
//         .EnableDetailedErrors()
// );
// builder.UseSqlite<GrapheCache>(options => options);
// Register Graphene Services after your DatabaseContext.
builder.Services.AddGraphene<GrapheneCache>(builder);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
// app.UseHttpsRedirection();
app.UseCors("development");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();