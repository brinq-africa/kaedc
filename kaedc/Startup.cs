﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using kaedc.Interfaces;
using kaedc.Models;
using kaedc.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace kaedc
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<Kaedc>(options =>
                options.UseMySql(Configuration.GetConnectionString("KaedcConnection")));

            services.AddIdentity<Kaedcuser, IdentityRole>(options => 
            {
                options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
            })
            .AddEntityFrameworkStores<Kaedc>()
            .AddDefaultTokenProviders();

            //services.ConfigureApplicationCookie(opt => {
            //    opt.LoginPath = "/identity/account/login";
            //    opt.ReturnUrlParameter = "RedirectUrl";
            //    opt.LogoutPath = "/identity/account/logout";
            //    //opt.AccessDeniedPath = "/Login/Index";
            //    //opt.ExpireTimeSpan = new TimeSpan(0, 15, 0);
            //});

            services.AddSwaggerGen(i =>
            {
               i.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info {
                   Title = "Brinq KAEDC",
                   Description = "API documentation for Brinq Africa's KAEDC application",
               });
            });

            //services.AddCors(config =>
            //{
            //    var policy = new CorsPolicy();
            //    policy.Headers.Add("*");
            //    policy.Methods.Add("*");
            //    policy.Origins.Add("*");
            //    policy.SupportsCredentials = true;
            //    config.AddPolicy("policy", policy);
            //});

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(cfg => 
            {
                cfg.SlidingExpiration = true;
                cfg.LoginPath = "/Identity/Account/Login";
                cfg.LogoutPath = "/Identity/Account/Logout";
                cfg.ExpireTimeSpan = new TimeSpan(0, 30, 15);
            })
            .AddJwtBearer(options =>
            {

                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<ITransaction, TransactionService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseExceptionHandler(appBuilder =>
            //{
            //    appBuilder.Use(async (context, next) =>
            //    {
            //        var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

            //        //when authorization has failed, should retrun a json message to client
            //        if (error != null && error.Error is SecurityTokenExpiredException)
            //        {
            //            context.Response.StatusCode = 401;
            //            context.Response.ContentType = "application/json";

            //            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            //            {
            //                State = "Unauthorized",
            //                Msg = "token expired"
            //            }));
            //        }
            //        //when orther error, retrun a error message json to client
            //        else if (error != null && error.Error != null)
            //        {
            //            context.Response.StatusCode = 500;
            //            context.Response.ContentType = "application/json";
            //            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            //            {
            //                State = "Internal Server Error",
            //                Msg = error.Error.Message
            //            }));
            //        }
            //        //when no error, do next.
            //        else await next();
            //    });
            //});

            //app.UseCors("policy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            CreateRoles(serviceProvider).Wait();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API");
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<Kaedcuser>>();
            string[] roleNames = { "SuperAdmin", "Admin", "Manager", "Agent", "Member" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Assigning SuperAdmin role to the main User here we have given our newly registered 
            //login id for SuperAdmin management
            Kaedcuser user = await UserManager.FindByEmailAsync("ogenwike@gmail.com");

            await UserManager.AddToRoleAsync(user, "SuperAdmin");
        }
    }



    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                string text = string.Format("Please click on this link to {0}: {1}", subject, message);
                string html = "Please confirm your account by clicking this link: <a href=\"" + message + "\">link</a><br/>";

                html += HttpUtility.HtmlEncode(@"Or copy paste the following link to your browser:" + message);

                SmtpClient client = new SmtpClient
                {
                    Host = "brinqafrica.com/webmail",
                    Credentials = new NetworkCredential("noreply@brinqafrica.com", "Starcomms@123")
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("noreply@brinqafrica.com")
                };
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;
                //mailMessage.Body = message;
                mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return Task.CompletedTask;
        }
    }
}
