using Arpl.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi.SampleApp.Domain.Errors;

namespace SampleWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<SampleWebApi.SampleApp.Domain.Model.IPersonRepository, SampleWebApi.SampleApp.Repositories.PersonRepository>();
            builder.Services.AddScoped<SampleWebApi.SampleApp.Application.PersonService>();


            //Configure ARPL AspNetCore integration
            ArplAspNetCore.Setup(options =>
            {
                // Custom error handling based on error type
                options.ErrorHandler = error => error switch
                {
                    ValidateError ve => new BadRequestObjectResult(ve),
                    NotFoundError nf => new NotFoundObjectResult(nf),
                    _ => new ObjectResult(error) { StatusCode = StatusCodes.Status500InternalServerError }
                };
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = 
                        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });
            
            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ARPL Sample API",
                    Version = "v1",
                    Description = "A sample API demonstrating ARPL usage."
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ARPL Sample API v1");
                    c.RoutePrefix = string.Empty; // Serve Swagger UI at the root URL
                });
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
