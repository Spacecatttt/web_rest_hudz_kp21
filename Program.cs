using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using web_rest_hudz_kp21.Database;
using web_rest_hudz_kp21.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using web_rest_hudz_kp21.Validators;
using web_rest_hudz_kp21.Database.Repositories;
using AutoMapper;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        #region Repositories
        builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IRepository<BikePart>, BikePartRepository>();
        builder.Services.AddScoped<IRepository<Bicycle>, BicycleRepository>();
        #endregion


        builder.Services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );


        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddValidatorsFromAssemblyContaining<BicycleValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<BikePartValidator>();
        builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI
        // at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "API Bike Shop",
                    Version = "v1"
                }
            );

            var filePath = Path.Combine(
                System.AppContext.BaseDirectory,
                "web_rest_hudz_kp21.xml"
            );
            c.IncludeXmlComments(filePath);
        });

        var app = builder.Build();

        // Ensure the database is created
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            dbContext.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
