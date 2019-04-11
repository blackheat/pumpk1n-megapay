using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using pumpk1n_backend.Helpers.Accounts;
using pumpk1n_backend.Mappings;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Services.Accounts;
using pumpk1n_backend.Settings;
using Swashbuckle.AspNetCore.Swagger;

namespace pumpk1n_backend
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using Bearer scheme. E.g: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                
                c.DescribeAllEnumsAsStrings();
            });
            
            // Add DatabaseContext with DB connection string
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (String.IsNullOrEmpty(connectionString))
                connectionString = _configuration.GetConnectionString("Pumpk1nDatabase");

            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
            
            // Configuring D-I for Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IInternalAccountService, InternalAccountService>();
            
            // Configuring D-I for Helpers
            services.AddScoped<IAccountHelper, AccountHelper>();
            
            // Load settings from appSettings
            var jwtSettingsSection = _configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);
            
            // Configure JWT Settings
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
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
