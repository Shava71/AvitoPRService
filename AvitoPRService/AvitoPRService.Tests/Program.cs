// AvitoPRService.Tests/Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AvitoPRService.Infrastructure.Data;
using System.Text.Json.Serialization;
using AvitoPRService.DependencyInjection.ServiceExtentions;
using AvitoPRService.Extensions.MiddlewareExtension;
using Microsoft.AspNetCore.Mvc.ApplicationParts;


var builder = WebApplication.CreateBuilder(args);

// InMemory для тестов
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")); 

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .PartManager.ApplicationParts.Add(
        new AssemblyPart(typeof(AvitoPRService.Api.Controllers.TeamController).Assembly));;

builder.Services.AddRepository();
builder.Services.AddServices();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.UseAdditionalExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AvitoPRService API V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();

namespace AvitoPRService.Tests
{
    public partial class Program {}
}