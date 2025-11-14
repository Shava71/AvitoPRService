using System.Text.Json.Serialization;
using AvitoPRService.DependencyInjection.ServiceExtentions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddRepository();

var app = builder.Build();


app.UseHttpsRedirection();



app.Run();
