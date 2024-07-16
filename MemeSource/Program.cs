using Microsoft.OpenApi.Models;
using System.Reflection;
using MemeSource.Models;
using MemeSource.Interfaces;
using MemeSource.Services;
using Serilog;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using MemeRepository.Db.Models;

namespace MemeSource
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Serilog
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            // Add services to the container.
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = $"An ASP.NET Core Web API for managing ToDo items {File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss zzz")}",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });
                // using System.Reflection;
                //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddDbContext<MemeRepositoryContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MainDB"));
                options.EnableSensitiveDataLogging(); //EF SQL Console
            });

            builder.Services.Configure<TwitterConfig>(builder.Configuration.GetSection("Twitter"));
            builder.Services.AddSingleton<ITwitterImageService, TwitterImageService>();
            builder.Services.AddHostedService<BackgroundImageFetchService>();
            builder.Services.AddSingleton<IBackgroundImageFetchService>(sp =>
                sp.GetRequiredService<IHostedService>() as BackgroundImageFetchService);
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                //app.UseSwagger(options =>
                //{   //use OpenAPI v2.0
                //    options.SerializeAsV2 = true;
                //});
                app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.MapControllers();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
