using Microsoft.Extensions.DependencyInjection;
using MovieApp.Interface;
using MovieApp.Services;

var builder = WebApplication.CreateBuilder(args);


 builder.Services.AddHttpClient();
 builder.Services.AddControllers();
builder.Services.AddSingleton<IOmdbService>(provider =>
{
    var httpClient = provider.GetService<HttpClient>();
    var configuration = provider.GetService<IConfiguration>();
    var apiKey = configuration["e2e2ba0"];
    return new OmdbService(httpClient, apiKey);
});

builder.Services.AddSingleton<ISearchQueryService, SearchQueryService>();
builder.Services.AddCors(options => options.AddPolicy("corspolicy",builder => 
{
    builder.WithOrigins( "http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
}));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "OmdbMovie API", Version = "v1" });
    });
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});

var app = builder.Build();
app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieApp");
        });
app.UseCors("corspolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
