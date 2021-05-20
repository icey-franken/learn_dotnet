using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //this is where we 'add framework services to the container'
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(MvcOptions => MvcOptions.EnableEndpointRouting = false);
            //MvcOptions.EnableEndpointRouting = false;
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
                app.UseExceptionHandler();
            }

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello, Borld!");
            });

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
