using System;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using ocr_api.Data;
using Swashbuckle.SwaggerGen.Generator;

namespace ocr_api
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostEnv;
        private readonly IApplicationEnvironment _appEnv;

        public Startup(IHostingEnvironment hostEnv, IApplicationEnvironment appEnv)
        {
            _hostEnv = hostEnv;
            _appEnv = appEnv;

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "OCR API",
                    Description = "A simple API for correcting OCR'd words"
                });
                c.DescribeAllEnumsAsStrings();
            });

            if (_hostEnv.IsDevelopment())
            {
                services.ConfigureSwaggerGen(c =>
                {                    
                    c.IncludeXmlComments(GetXmlCommentsPath());
                });
            }

            services.AddOptions();
            services.Configure<Options.Settings>(Configuration);

            services.AddTransient<IWordRepository, WordRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();

            app.UseSwaggerGen();
            app.UseSwaggerUi();
        }

        private string GetXmlCommentsPath()
        {
            var dir = Directory.CreateDirectory(_appEnv.ApplicationBasePath);
            while (dir.Parent != null)
            {
                if (dir.GetFiles("global.json").Any())
                {
                    var basePath = dir.FullName;
                    var cfgName = _appEnv.Configuration;
                    var runtime = _appEnv.RuntimeFramework;
                    var rtName = runtime.Identifier;
                    var rtVer = runtime.Version.ToString().Replace(".", string.Empty);
                    var path = $@"{basePath}\artifacts\bin\ocr-api\{cfgName}\{rtName}{rtVer}\ocr-api.xml";
                    return path;
                }

                dir = dir.Parent;
            }
            throw new InvalidOperationException("Failed to detect solution base path - global.json not found.");
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}
