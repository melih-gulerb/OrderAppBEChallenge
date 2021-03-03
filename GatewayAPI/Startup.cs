using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace GatewayAPI
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
            services.AddOcelot(Configuration);
        var jwtConfig = Configuration.GetSection("Audience");
        
        var regKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["SecretKey"]));
        // Take the part of json file named "Audience" assign given "SecretKey" to recKey variable
        var tokenValidationParameters = new TokenValidationParameters //Token specs that will be given in Authorization project
        {   
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Aud"],
            IssuerSigningKey = regKey,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,   //Enable expiration time (defined a long time..)
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(a =>
        {
            a.DefaultAuthenticateScheme = "Key";
        });

        services.AddAuthentication().AddJwtBearer("Key", k =>
        { k.RequireHttpsMetadata = false; k.TokenValidationParameters = tokenValidationParameters; });
        
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            await app.UseOcelot();
        }
    }
}