using System.Text.Json.Serialization;
using AvitoPRService.DependencyInjection.ServiceExtentions;
using AvitoPRService.Extensions.MiddlewareExtension;
using AvitoPRService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    // string connectionStringWithTimeout = connectionString + ";Timeout=30;Command Timeout=30";
    
    options.UseNpgsql(connectionString, 
        npgsqlOptions => 
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null);
        });
});



builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen(c =>
{
    try
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "AvitoPRService", Version = "v1" });

        //Добавляем схему авторизации
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Введите токен в формате: Bearer {токен}",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        // //Добавляем требование авторизации по умолчанию
        // c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        // {
        //     {
        //         new OpenApiSecurityScheme
        //         {
        //             Reference = new OpenApiReference
        //             {
        //                 Type = ReferenceType.SecurityScheme,
        //                 Id = "Bearer"
        //             },
        //             Scheme = "oauth2",
        //             Name = "Bearer",
        //             In = ParameterLocation.Header,
        //         },
        //         new List<string>()
        //     }
        // });
    }
    catch (Exception ex)
    {
        Console.WriteLine("Swagger ex: " + ex.Message);
        throw;
    }
});

// DI
builder.Services.AddRepository();
builder.Services.AddServices();

var app = builder.Build();

if (args.Contains("--migrate"))
{
    using var serviceScope = app.Services.CreateScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    context.Database.Migrate();
    
    string sql = File.ReadAllText("sql/triggers.sql");
    context.Database.ExecuteSqlRaw(sql);

    return;
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors();
app.UseAdditionalExceptionHandler(); //собственный middleware обработки ошибок
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AvitoPRService API V1");
    c.RoutePrefix = string.Empty;
});
app.MapControllers();
app.UseHttpsRedirection();

// using (var serviceScope = app.Services.CreateScope())
// {
//     var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
//     context.Database.Migrate();
//
//     string sql = File.ReadAllText("sql/triggers.sql");
//     context.Database.ExecuteSqlRaw(sql);
// }

app.Run();

namespace AvitoPRService.Api
{ 
    public partial class Program { }
}

