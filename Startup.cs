using AutoMapper;
using HumSafar.API.Auth;
using HumSafar.API.EmailServices;
using HumSafar.BL.Interface;
using HumSafar.BL.Services;
using HumSafar.DL;
using HumSafar.DL.Entities;
using HumSafar.DL.Repos.Implementation;
using HumSafar.DL.Repos.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrendyKart.API.Services;

namespace HumSafar.API
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
            services.AddAutoMapper(typeof(Startup));
            AddSwagger(services);
            RegisterDbServices(services);
            RegisterBusinessServices(services);
            RegisterConfigurations(services);
            AddJwtAuthentication(services);
        }

        public void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "1.0.0",
                    Title = "HumSafar",
                    Description = "Best Travel Package",
                    TermsOfService = new Uri("https://github.com/"),
                    License = new OpenApiLicense
                    {
                        Name = "MIT"
                    },
                    Contact = new OpenApiContact
                    {
                        Email = "ankit.prasad@blueconchtech.com",
                        Name = "",
                        Url = new Uri("https://www.linkedin.com/in/")
                    }
                });

                config.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                var req = new OpenApiSecurityRequirement();
                req.Add(scheme, new List<string>());
                config.AddSecurityRequirement(req);

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var path = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                config.IncludeXmlComments(path);
            });
        }

        private void RegisterDbServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString"));

            });
            services.AddScoped<ITourRepo, TourRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
        }

        private void RegisterConfigurations(IServiceCollection services)
        {
            services.Configure<JwtConfig>(Configuration.GetSection("Jwt"));
            services.Configure<EmailSendGridConfig>(Configuration.GetSection("EmailSendGrid"));
        }
        private void RegisterBusinessServices(IServiceCollection services)
        {
            services.AddScoped<IEmailSender, MockEmailSender>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IEmailSender, MockEmailSender>();

            // Register Identity
            services.AddIdentity<HumSafarUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>().
                AddDefaultTokenProviders();
        }

        private void AddJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
          .AddJwtBearer(options =>
          {
              var secret = Configuration.GetSection("Jwt").GetValue<string>("Secret");
              options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
              {
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                  ValidateAudience = false,
                  ValidateIssuer = false,
              };
              options.RequireHttpsMetadata = false;
          });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "HumSafar");
            });

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(options =>
            {
                options.AllowAnyMethod();
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
