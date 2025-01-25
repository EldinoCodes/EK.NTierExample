
using EK.NTierExample.Api.Data.Context;
using EK.NTierExample.Api.Data.Repositories.Addresses;
using EK.NTierExample.Api.Services.Addresses;
using System.Security.Principal;

namespace EK.NTierExample.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddTransient<IPrincipal>(s =>
            {
                var principal = s.GetService<IHttpContextAccessor>()?.HttpContext?.User;
                if (!principal?.Claims?.Any() ?? false) principal = null;

                principal ??= new GenericPrincipal(new GenericIdentity("Anonymous"), []);

                return principal;
            });

        // Add services to the container.
        builder.Services.AddScoped<INTierExampleContext, NTierExampleContext>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IAddressService, AddressService>();


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
