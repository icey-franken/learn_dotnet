using CityInfo.API.Context;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;

namespace CityInfo.API
{
    public class Startup
    {
        //use dependency injection of the configuration object to get configuration variables (e.g. connection string)
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //this is where we 'add framework services to the container'
        public void ConfigureServices(IServiceCollection services)
        {
            //https://stackoverflow.com/questions/57684093/using-usemvc-to-configure-mvc-is-not-supported-while-using-endpoint-routing
            services.AddMvc(MvcOptions => MvcOptions.EnableEndpointRouting = false)
                .AddMvcOptions(o =>
                {
                    o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                });

            //had to add these along with NewtonsoftJson package from NuGet.
            //  see here for info: https://github.com/dotnet/aspnetcore/issues/13938
            services
                .AddControllersWithViews()
                .AddNewtonsoftJson();
            //MvcOptions.EnableEndpointRouting = false;
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            var connectionString = _configuration["connectionStrings:cityInfoDBConnectionString"];
            //"registers city info context with a scoped lifetime."
            services.AddDbContext<CityInfoContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            // "add scoped lifetime for a repository so it's created once per request"
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //this is where we add middlewares
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePages();

            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello, Borld!");
            //});

            //routing matches request URI to controller METHOD
            //two types of routing: convention based and attribute based - we will use attribute based.
            //  convention: need to be configured; typically used if returning HTML views. Not rec for APIs
            //  attribute: allows use of attributes at controller and action level; can have optional template.
            //    URI matched to a specific action on a controller
            //    there are controller level and action level attributes
            //      action level attributes (HttpGet, HttpPost, etc.) are members of a controller level attribute (route)
            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello Borld!");
            //    });
            //});
        }
    }
}
