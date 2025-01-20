
using EK.NTierExample.Api.Data.Context;
using EK.NTierExample.Api.Data.Repositories.WeatherForecasts;
using EK.NTierExample.Api.Services.WeatherForecasts;

namespace EK.NTierExample.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<INTierExampleContext, NTierExampleContext>();
        builder.Services.AddScoped<IWeatherSummaryRepository, WeatherSummaryRepository>();
        builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
    }
}
