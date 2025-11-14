using System.Text.Json.Serialization;
using AvitoPRService.DependencyInjection.ServiceExtentions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// DI
builder.Services.AddRepository();
builder.Services.AddServices();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseExceptionHandler(); //собственный middleware обработки ошибок


app.Run();
