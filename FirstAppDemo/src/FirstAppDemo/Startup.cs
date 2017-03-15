using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Routing;
using FirstAppDemo.Models; // for FirstAppDemoDbContext class.
using Microsoft.EntityFrameworkCore; // for option.UseSqlServer()

namespace FirstAppDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加 MVC 服务。
            services.AddMvc();
            // 添加 Entity Framework 服务，并且使用SQL Server  服务。
            services.AddEntityFrameworkSqlServer().AddDbContext<FirstAppDemoDbContext>
                (option => option.UseSqlServer(Configuration["database:connection"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 包含 UseDefaultFiles 和 UseStaticFiles 两个中间件。
            app.UseFileServer();
            // 添加 MVC 中间件。
            app.UseMvc(ConfigureRoute);

            // 使用Run 注册的中间件不能呼叫另一个中间件，它必须对请求作出回应。
            // 它是最后一个中间件。
            app.Run(async (context) =>
            {
                var msg = Configuration["message"];
                await context.Response.WriteAsync(msg);
            });
        }

        private void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            //Home/Index
            routeBuilder.MapRoute("Default",
                "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
