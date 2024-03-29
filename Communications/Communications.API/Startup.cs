﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Communications.API.Data;
using Communications.API.Helpers;
using Hangfire;
//using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Communications.API
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

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            //add hangfire server 1
            services.AddHangfireServer(x => x.ServerName = "Server 1");

            //add hangfire server 2
            services.AddHangfireServer(x => x.ServerName = "Server 2");

            //add hangfire server 3
            services.AddHangfireServer(x => x.ServerName = "Server 3");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<MainDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("PrimaryConnection"));
            }, optionsLifetime: ServiceLifetime.Transient);
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseHangfireDashboard("/hangfire");
                       
            app.Use(async (context, next) =>
            {

                bool isCommunicationsEndpoint = context.Request.Path.Value.ToLower().Contains("api/communications");

                if (isCommunicationsEndpoint)
                {
                    var hasVendorKeyHeader = context.Request.Headers.Any(x => x.Key.ToLower() == "x-vendor-key");

                    if (hasVendorKeyHeader)
                    {
                        var vendorKey = context.Request.Headers.Where(x => x.Key.ToLower() == "x-vendor-key").FirstOrDefault().Value;

                        using (var serviceScope = app.ApplicationServices.CreateScope())
                        {
                            var services = serviceScope.ServiceProvider;
                            MainDbContext mainDbContext = services.GetService<MainDbContext>();

                            VendorHelper vendorHelper = new VendorHelper(mainDbContext);

                            var parsedVendorKey = Guid.Parse(vendorKey);

                            var vendorExists = vendorHelper.VendorExists(parsedVendorKey);

                            if (vendorExists)
                            {
                                context.Items.Add(key: "VendorId", value: vendorKey);

                                await next.Invoke();
                            }
                            else
                            {
                                context.Response.StatusCode = 401;
                                await context.Response.WriteAsync("No Vendor with the key provided exists. Unauthorized.");
                            }
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("No X-Vendor-Key header found on the HTTP request. Unauthorized.");
                    }
                }
                else
                {
                    await next.Invoke();
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CustomInitializer.Initialize(app.ApplicationServices);

            RegisterApplicationDefaults();
        }

        private void RegisterApplicationDefaults()
        {
            BackgroundJobHelper.AddDefaultHangfireProcesses();
        }
    }


    public class CustomInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MainDbContext(serviceProvider.GetRequiredService<DbContextOptions<MainDbContext>>()))
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
