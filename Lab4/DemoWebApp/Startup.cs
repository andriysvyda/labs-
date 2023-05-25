using DemoWebApp.Models;
using DemoWebApp.Services.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DemoWebApp.Handlers;
using DemoWebApp.Services;

namespace DemoWebApp
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<IRepository<User>, UsersRepositoryService>();
            services.AddScoped<IRepository<Post>, PostsRepositoryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAuthentication("JwtBearerAuthentication")
                .AddScheme<JwtBearerAuthenticationOptions, JwtBearerAuthenticationHandler>("JwtBearerAuthentication", options =>
                {
                    options.JwtKey = Configuration["AppSettings:JwtKey"];
                    options.JwtIssuer = Configuration["AppSettings:JwtIssuer"];
                });
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
