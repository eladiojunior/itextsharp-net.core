using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace itextsharp_net.core
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Gerador de Arquivo Pdf - iTextSharp Core",
                    Version = "v1",
                    Description = "APIs REST para geração e consulta de arquivos Pdf utilizando o iTextSharp e .Net Core",
                    Contact = new OpenApiContact
                    {
                        Name = "Eladio Júnior",
                        Email = "eladiojunior@gmail.com",
                        Url = new Uri("https://github.com/eladiojunior")
                    }
                });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1 Geração e consulta de arquivos Pdf");
            });
        }
    }
}