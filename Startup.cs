using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Arfler.Services;
using Arfler.Models;
using Arfler.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Pioneer.Pagination;

namespace Arfler
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
            var connection = Configuration.GetConnectionString("ArflerDatabase");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
            services.AddDbContext<ArflerDBContext>(options => options.UseSqlServer(connection));
            services.AddMemoryCache();

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // Set up mechanism for sending email.
            IConfigurationSection sectionEmailProvider = Configuration.GetSection("EmailProvider");

            string emailProvider = sectionEmailProvider["Provider"];

            string emailConnectionType = sectionEmailProvider["ConnectionType"];

            if (null == emailProvider)
            {
                throw new ArgumentNullException(
                    "EmailProvider",
                    "Missing email service provider configuration"
                );
            }

            IConfiguration emailConfig = LoadEmailConfig();

            switch (emailProvider.ToLower())
            {
                case "mailgun":
                    Console.WriteLine(
                        "Starting send mail via MailGun email service...");

                    if ("api" == emailConnectionType.ToLower())
                    {

                        services.AddTransient<IEmailSender,
                            MailGunApiEmailSender>();

                        services.Configure<MailGunApiEmailSettings>(
                            emailConfig);

                    }
                    if ("smtp" == emailConnectionType.ToLower())
                    {

                        services.AddTransient<IEmailSender,
                            MailGunSmtpEmailSender>();

                        services.Configure<MailGunSmtpEmailSettings>(
                            emailConfig);

                    }
                    break;
                default:
                    Console.WriteLine("Error: Unknown email service.");
                    throw new ArgumentException(
                        "Unknown email service provider in configuration.",
                        "EmailProvider:Provider"
                    );
                    // break;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseSession();
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        // 
        // Load Mailgun configurations based on connection type.
        // 
        protected IConfiguration LoadEmailConfig()
        {

            IConfigurationSection sectionEmailProvider =
                Configuration.GetSection("EmailProvider");

            string emailProvider = sectionEmailProvider["Provider"];

            if (null == emailProvider)
            {

                throw new ArgumentException(
                    "Empty email provider.",
                        "EmailProvider:Provider"
                );

            }

            IConfiguration emailConfig = null;
            switch (emailProvider.ToLower())
            {
                case "mailgun":
                    Console.WriteLine("Loading MailGun configuration...");

                    emailConfig = LoadMailGunEmailConfig();

                    break;
                default:
                    throw new ArgumentException(
                        "Unknown email provider.",
                        "EmailProvider:Provider"
                    );
                    // break;
            }

            return emailConfig;

        }
        protected IConfiguration LoadMailGunEmailConfig()
        {

            IConfigurationSection sectionEmailProvider =
                Configuration.GetSection("EmailProvider");

            string emailConnectionType =
                sectionEmailProvider["ConnectionType"];

            IConfiguration emailConfig = null;

            if (null == emailConnectionType)
            {

                throw new ArgumentException(
                    "Invalid email connection type.",
                    "EmailProvider:ConnectionType"
                );

            }
            switch (emailConnectionType.ToLower())
            {
                case "api":
                    emailConfig = LoadMailGunApiEmailSettings();
                    break;
                case "smtp":
                    emailConfig = LoadMailGunSmtpEmailSettings();
                    break;

                default:

                    throw new ArgumentException(
                        "Unknown email connection type.",
                        "EmailProvider:ConnectionType"
                    );

                    // break;
            }
            return emailConfig;
        }

        //
        // Configure email sender based on Mailgun configurations for REST API.
        // 
        protected IConfiguration LoadMailGunSmtpEmailSettings()
        {

            IConfigurationSection sectionEmailProvider =
                Configuration.GetSection("EmailProvider");

            string configFile = sectionEmailProvider["ConfigFile"];

            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration config = builder.Build();

            IConfigurationSection sectionSmtp = config.GetSection("SMTPSettings");
            return sectionSmtp;
        }

        //
        // Configure email sender based on Mailgun configurations for Smtp.
        // 
        protected IConfiguration LoadMailGunApiEmailSettings()
        {

            IConfigurationSection sectionEmailProvider =
                Configuration.GetSection("EmailProvider");

            string configFile = sectionEmailProvider["ConfigFile"];

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration config = builder.Build();

            IConfigurationSection sectionRestApi =
                config.GetSection("RestAPISettings");

            return sectionRestApi;

        }
    }
}
