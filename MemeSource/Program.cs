using Microsoft.OpenApi.Models;
using System.Reflection;
using MemeSource.Models;
using MemeSource.Interfaces;
using MemeSource.Services;
using Serilog;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using MemeRepository.Db.Models;
using MemeSource.Repositories;
using MemeSource.DAL.Interfaces;
using Hangfire;
using Hangfire.SqlServer;

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
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

            builder.Services.AddDbContext<MemeRepositoryContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MainDB"));
                options.EnableSensitiveDataLogging(); //EF SQL Console
            });

            builder.Services.Configure<TwitterConfig>(builder.Configuration.GetSection("Twitter"));
            builder.Services.AddHostedService<BackgroundImageFetchService>();

            builder.Services.AddSingleton<ITwitterImageService, TwitterImageService>();
            builder.Services.AddSingleton<IBackgroundImageFetchService>(sp =>
                sp.GetRequiredService<IHostedService>() as BackgroundImageFetchService);
            builder.Services.AddScoped<ISystemPropertyRepository, SystemPropertyRepository>();
            //builder.Services.AddRazorPages();
            builder.Services.AddMvc(); // AddControllersWithViews() & AddRazorPages()
            //Hangfire Service
            builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("MainDB"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

            var app = builder.Build();
            // HangFire Dashboard
            app.UseHangfireDashboard();
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

            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=SystemProperty}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
