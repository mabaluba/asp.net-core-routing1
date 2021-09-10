using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace less4_lasthomework
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration AppConfiguration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath);
            builder.AddJsonFile("books.json");
            AppConfiguration = builder.Build();
            /* var sections = AppConfiguration.GetSection("books");
             var section = sections.GetChildren();*/

            var myRouteHandlerLibrary = new RouteHandler(HandlerLibrary);
            var myRouteHandlerLibraryBooks = new RouteHandler(HandlerLibraryBooks);
            var myRouteHandlerLibraryProfile = new RouteHandler(HandlerLibraryProfile);


            var routeBuilderLibrary = new RouteBuilder(app, myRouteHandlerLibrary);
            var routeBuilderLibraryBooks = new RouteBuilder(app, myRouteHandlerLibraryBooks);
            var routeBuilderLibraryProfile = new RouteBuilder(app, myRouteHandlerLibraryProfile);

            //var routeBuilderLibraryBooks = new RouteBuilder(app, new RouteHandler(HandlerLibrary)),
            //app.UseRouter(new RouteBuilder(app, new RouteHandler(HandlerLibraryBooks)).Build());

            routeBuilderLibrary.MapRoute("default", "Library");
            routeBuilderLibraryBooks.MapRoute("default", "Library/Books");
            routeBuilderLibraryProfile.MapRoute("default", "Library/Profile/{id:range(0,5)?}");//, constraints: new {id=5});//{id::range(0,5)

            app.UseRouter(routeBuilderLibrary.Build());
            app.UseRouter(routeBuilderLibraryBooks.Build());
            app.UseRouter(routeBuilderLibraryProfile.Build());
            


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                /*endpoints.MapGet("/Library", async context =>
                {
                    await context.Response.WriteAsync("Hello Client!");
                });
                
                endpoints.MapGet("Library/Books", async context =>
                {
                    foreach (var sec in section)
                    {
                        
                        await context.Response.WriteAsync($"<br>{sec.Key} - {sec.Value}</br>");
             B       }
                });
                endpoints.MapGet("/Library/Profile/{id?:range(0,5)}", async context =>
                {
                    await context.Response.WriteAsync("Hello Anonimous Client!");
                });*/
            });
        }

        private async Task HandlerLibraryProfile(HttpContext context)
        {
            var a = context.GetRouteValue("id").ToString();
            switch (a)
            {
                case "0": 
                    await context.Response.WriteAsync($"<br>{a} ноль</br>");
                    break;
                case "1": 
                    await context.Response.WriteAsync($"<br>{a} один</br>");
                    break;
                case "2": 
                    await context.Response.WriteAsync($"<br>{a} два</br>");
                    break;
                default:
                    await context.Response.WriteAsync($"<br>{a}</br>");
                    break;
            }
            
        }
        private async Task HandlerLibrary(HttpContext context)
        {
            await context.Response.WriteAsync("HandlerLibrary!");
        }
        private async Task HandlerLibraryBooks(HttpContext context)
        {
            var sections = AppConfiguration.GetSection("books");
            var section = sections.GetChildren();
            foreach (var sec in section)
            {
                await context.Response.WriteAsync($"<br>{sec.Key} - {sec.Value}</br>");
            }
        }


    }
}
