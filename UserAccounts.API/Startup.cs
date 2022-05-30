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
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAccount.Entities.DataAccess;
using UserAccount.Entities.Models;
using UserAccount.Service;
using UserAccount.Service.Interfaces;

namespace UserAccounts.API
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
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Alat Users",
                        Version = "v1",
                        Description = "API",
                        Contact = new OpenApiContact
                        {
                            Name = "Alat",
                            Email = "Info@alat.com"
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://en.wikipedia.org/wiki/MIT_Lincense")
                        }
                    });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using Bearer scheme. \r\n\r\n" +
                    "Enter 'Bearer' [space] and then your token in the input below.\r\n\r\n" +
                    "Example: \"Bearer 123456\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }

                });

                //var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);

                ////xml documentation
                //c.IncludeXmlComments(xmlCommentsFullPath);
            });
            services.AddDataProtection();
            services.AddCors(x => x.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin()
                                                                    .AllowAnyMethod()
                                                                    .AllowAnyHeader()));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IUserServices, UserServices>();

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(
                  Configuration.GetConnectionString("Default"),
                  b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireDigit = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequiredLength = 6;
                option.Password.RequireLowercase = false;
                option.SignIn.RequireConfirmedEmail = false;
                option.SignIn.RequireConfirmedPhoneNumber = false;


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
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alat Users v1");
                c.RoutePrefix = String.Empty;
            });

            app.UseHsts();
            app.Use(async (context, next) =>
            {
                //context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers["X-Xss-Protection"] = "1";
                //context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                //context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                //context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers["Referrer-Policy"] = "no-referrer";
                //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; report-uri /cspreport");
                //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; img-src https://*; child-src ?none?; ");
                await next();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
