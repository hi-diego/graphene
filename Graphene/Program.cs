using Graphene.Database;
using GrapheneCore.Database.Interfaces;
using GrapheneCore.Graph;
using GrapheneCore.Graph.Interfaces;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddScoped<IGrapheneDatabaseContext>((IServiceProvider provider) => provider.GetService<DatabaseContext>());
builder.Services.AddSingleton<IGraph>((IServiceProvider provider) => new Graph(new DatabaseContext()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => {
    // options.Filters.Add(typeof(DefaultExceptionFilter));
}).AddNewtonsoftJson(opt => {
    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    opt.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK";
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
