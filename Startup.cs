using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TodoBackend.Models;
using TodoBackend.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;



namespace TodoBackend
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
            services.AddCors(o => o.AddPolicy("AllowPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            SetupJWTServices(services);
            services.AddControllers();
            services.AddDbContext<DbContextBase>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 11))));
            // services.AddTransient<AppDb>(_ => new AppDb(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IEmailService, EmailService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowPolicy");


            app.UseRouting();

            app.UseAuthentication();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SetupJWTServices(IServiceCollection services)
        {
            string key = "my_secret_key_12345"; //this should be same which is used while creating token      
            var issuer = "http://mysite.com";  //this should be same which is used while creating token  

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = issuer,
                  ValidAudience = issuer,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
              };

              options.Events = new JwtBearerEvents
              {
                  OnAuthenticationFailed = context =>
                  {
                      if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                      {
                          context.Response.Headers.Add("Token-Expired", "true");
                      }
                      return Task.CompletedTask;
                  }
              };
          });
        }

    }
}
