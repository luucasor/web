using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
          //Transient: objects are always different; a new instance is provided to every controller and every service. 
          //services.AddTransient<ICatalogo, Catalogo>();
          //services.AddTransient<IRelatorio, Relatorio>();

          //Scoped: objects are the same within a request, but different across different requests.
          //services.Scoped<ICatalogo, Catalogo>();
          //services.Scoped<IRelatorio, Relatorio>();

          //Singleton: objects are the same for every object and every request.
          var catalogo = new Catalogo();
          services.AddSingleton<ICatalogo>(catalogo);
          services.AddSingleton<IRelatorio>(new Relatorio(catalogo));
        }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            IRelatorio relatorio = serviceProvider.GetService<IRelatorio>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                  await relatorio.Imprimir(context);
                });
            });
        }
    }
}
