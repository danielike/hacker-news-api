using Asp.Versioning;
using HackerNewsApi.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient(nameof(HttpClients.HackerNews),client =>
{
    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com");
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallback(() => Results.Problem(statusCode: StatusCodes.Status404NotFound, title: "Not Found"));

app.Run();