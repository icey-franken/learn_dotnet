using CourseManager.API.DbContexts;
using CourseManager.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseManager.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<AuthorRepository>();

            services.AddDbContext<CourseContext>(options =>
            {
                //Basic db setup when using sql server.
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CourseManagerDB;Trusted_Connection=True;");

                //We can use the in memory database like so during unit testing. Pass in the db name.
                //Are migrations performed automatically?
                //Will we need logic that migrates/seeds db according to which db provider we use?
                //options.UseInMemoryDatabase("CourseManagerInMemoryDB");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
