using GrapheneTemplate.Database;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Graphene.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Graphene.Services;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Graphene.Cache;

var builder = WebApplication.CreateBuilder(args);
// Register DatabaseContext Mysql
var connectionString = builder.Configuration.GetConnectionString("mysql");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
builder.Services.AddDbContext<GrapheneCache>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
// Register Graphene Services after your DatabaseContext.
builder.Services.AddGraphene<GrapheneCache>(builder);
builder.Services.AddMvc(options => {
    // Use the default DefaultExceptionFilter so we can throw StatusCodeException handly in any part of the app
    // this will handle it and return the correspondet result
    options.Filters.Add(typeof(SampleResultFilter));
    var f = options.OutputFormatters;
    // options.OutputFormatters.Add(typeof(RedisTextJsonOutputFormatter));
});
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

public class SampleResultFilter : IResultFilter
{
    private IConnectionMultiplexer _multiplexer { get; set; }

    public SampleResultFilter (IConnectionMultiplexer multiplexer, IEntityContext ec)
    {
        _multiplexer = multiplexer;
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        // Do something after the result executes.
        var result = (ObjectResult) context.Result;
        string input = JsonConvert.SerializeObject(result.Value);
        string output = new RedisGuidCache(_multiplexer).ReplaceIdsWithGuids(input);
        result.Value = JObject.Parse(output);
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}