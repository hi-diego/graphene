using GrapheneTemplate.Database;
using Graphene.Database.Interfaces;
using Graphene.Graph;
using Graphene.Graph.Interfaces;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Graphene.Extensions;
using Microsoft.AspNetCore.Identity;
using GrapheneTemplate.Database.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
// Register DatabaseContext Mysql
var connectionString = builder.Configuration.GetConnectionString("mysql");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
builder.Services.AddDbContext<GrapheneTemplate.Database.GrapheneCache>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
builder.Services
    .AddDefaultIdentity<Author>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<GrapheneCache>();
//builder.Services
    //.AddIdentityServer();
    //.AddApiAuthorization<Author, GrapheneCache>();
//builder.Services
//    .AddAuthentication()
//    .AddIdentityServerJwt();
builder.Services
    .AddControllersWithViews();
builder.Services
    .AddRazorPages();
// Register Graphene Services after your DatabaseContext.
builder.Services
    .AddGraphene<GrapheneCache>(builder);
builder.Services
    .AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseMigrationsEndPoint();
} else {
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
// app.UseIdentityServer();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();
app.Run();
