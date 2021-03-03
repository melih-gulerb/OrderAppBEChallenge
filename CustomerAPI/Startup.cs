using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.Model;
using Entities.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;


namespace CustomerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddDbContext<DataContext>(opt =>   //Use a database settings given in json file (appsettings.json)
                opt.UseSqlServer(Configuration.GetConnectionString("PathDB")));
            services.AddRepositoryService();    //Add Repo services to use at web api
            services.AddOcelot(Configuration);  // use configuration for Gateway  

            var jwtConfig = Configuration.GetSection("Audience");   // Take the part of json file named "Audience"
            var recKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig["SecretKey"])); // assign given
                                                                                                    // "SecretKey" to recKey variable
            var tokenValidationParameters = new TokenValidationParameters //Token specs will be given in Authorization project
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = recKey,
                ValidateIssuer = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtConfig["Aud"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "Key";
            });

            services.AddAuthentication().AddJwtBearer("Key", jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}