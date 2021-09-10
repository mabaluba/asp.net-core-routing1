using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            var sections = AppConfiguration.GetSection("books");
            var section = sections.GetChildren();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                
                
                endpoints.MapGet("/Library", async context =>
                {
                    await context.Response.WriteAsync("Hello Client!");
                });
                
                endpoints.MapGet("Library/Books", async context =>
                {
                    foreach (var sec in section)
                    {
                        
                        await context.Response.WriteAsync($"<br>{sec.Key} - {sec.Value}</br>");
                    }
                    
                });
            });
        }
    }
}
