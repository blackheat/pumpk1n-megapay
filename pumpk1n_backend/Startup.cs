using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using pumpk1n_backend.Mappings;
using Swashbuckle.AspNetCore.Swagger;

namespace pumpk1n_backend
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add support for AutoMapper with IHaveCustomMappings, IMapFrom and IMapTo interface
            // Load all mapping assemblies from classes implementing at least one of mentioned interfaces.
            var maps = AutoMapperConfigurator.LoadMapsFromAssemblies();
            services.AddSingleton(maps);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1.0.0-beta",
                    Title = "Pumpk1n API",
                    Description = "Pumpk1n API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Pumpk1n Developers",
                        Email = "xuongvuong11@gmail.com",
                        Url = "https://www.pumpk1n.xyz"
                    },
                    License = new License
                    {
                        Name = "Use under permission of Pumpk1n",
                        Url = "https://www.pumpk1n.xyz/license"
                    }
                });

                c.DescribeAllEnumsAsStrings();
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pumpk1n API V1");
                c.RoutePrefix = String.Empty;
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
