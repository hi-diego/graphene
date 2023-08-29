using GrapheneTemplate.Database;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Graphene.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Graphene.Services;
using StackExchange.Redis;
using System.Buffers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.ObjectPool;

using System.Text;


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
builder.Services.TryAddEnumerable(
    ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, NewtonsoftJsonMvcOptionsSetup>()
);
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


public class RedisTextJsonOutputFormatter : Microsoft.AspNetCore.Mvc.Formatters.NewtonsoftJsonOutputFormatter
{
    public RedisTextJsonOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool, MvcOptions mvcOptions, MvcNewtonsoftJsonOptions? jsonOptions)
        : base  (serializerSettings, charPool, mvcOptions, jsonOptions)
    {
        var foo = "test";
    }

    public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var r = base.WriteResponseBodyAsync(context, selectedEncoding);
        return r;
    }
}



/// <summary>
/// Sets up JSON formatter options for <see cref="MvcOptions"/>.
/// </summary>
internal sealed class NewtonsoftJsonMvcOptionsSetup : IConfigureOptions<MvcOptions>
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly MvcNewtonsoftJsonOptions _jsonOptions;
    private readonly ArrayPool<char> _charPool;
    private readonly ObjectPoolProvider _objectPoolProvider;

    public NewtonsoftJsonMvcOptionsSetup(
        ILoggerFactory loggerFactory,
        IOptions<MvcNewtonsoftJsonOptions> jsonOptions,
        ArrayPool<char> charPool,
        ObjectPoolProvider objectPoolProvider)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentNullException.ThrowIfNull(jsonOptions);
        ArgumentNullException.ThrowIfNull(charPool);
        ArgumentNullException.ThrowIfNull(objectPoolProvider);

        _loggerFactory = loggerFactory;
        _jsonOptions = jsonOptions.Value;
        _charPool = charPool;
        _objectPoolProvider = objectPoolProvider;
    }

    public void Configure(MvcOptions options)
    {
        // options.OutputFormatters.RemoveType<NewtonsoftJsonOutputFormatter>();
        options.OutputFormatters.Add(new RedisTextJsonOutputFormatter(_jsonOptions.SerializerSettings, _charPool, options, _jsonOptions));

        // options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
        // Register JsonPatchInputFormatter before JsonInputFormatter, otherwise
        // JsonInputFormatter would consume "application/json-patch+json" requests
        // before JsonPatchInputFormatter gets to see them.
    }
}

public class SampleResultFilter : IActionFilter
{
    public ConfigurationOptions Options { get; set; }
    public IEntityContext EC { get; set; }
    public IDatabase Redis { get; set; }
    public SampleResultFilter (IConfiguration configuration, IEntityContext ec)
    {
        Options = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
        EC = ec;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Do something before the result executes.
        ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(Options);
        Redis = connection.GetDatabase();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do something after the result executes.
        var result = (OkObjectResult) context.Result;
        // byte[] byteArray = Encoding.UTF8.GetBytes("FOOO");
        // var newBody =  new MemoryStream(byteArray);
        // context.HttpContext.Response.Body = newBody;
        var value = Redis.HashGetAll(new RedisKey("GrapheneCacheBill-37"))?.LastOrDefault().Value.ToString();
        result.Value = result.Value.ToString().Replace("RedisReplace-Bill-2", value);
        // var content = context.HttpContext.Response.
        // content.Body.Position = 0;
        // using (StreamReader reader = new StreamReader(content.Body, Encoding.UTF8))
        // {
        //     var body = reader.ReadToEnd();
        // }
        // result.Formatters.Add(new RedisTextJsonOutputFormatter);
        // var uuids = Redis.StringGet(EC.RedisKeys.Keys.Select(k => new RedisKey(k)).ToArray());
    }
}