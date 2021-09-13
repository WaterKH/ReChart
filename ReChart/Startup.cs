using BlazorDownloadFile;
using Blazored.Modal;
using Blazored.Toast;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReChart.Interfaces;
using ReChart.Logic;
using ReChart.Services;

namespace ReChart
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<IFieldChartService>(new FieldChartService());
            services.AddSingleton<IMemoryChartService>(new MemoryChartService());
            services.AddSingleton<IBossChartService>(new BossChartService());

            services.AddSingleton<ElectronService>();

            services.AddServerSideBlazor().AddCircuitOptions(o =>
            {
                if (this.Environment.IsDevelopment()) //only add details when debugging
                {
                    o.DetailedErrors = true;
                }
            });

            services.AddBlazorDownloadFile();
            services.AddBlazoredToast();
            services.AddBlazoredModal();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            if (HybridSupport.IsElectronActive)
            {
                CreateWindow();
            }

            Settings.LoadSettings();
        }

        private static async void CreateWindow()
        {
            var options = new BrowserWindowOptions
            {
                AutoHideMenuBar = true,
                DarkTheme = true,
                FullscreenWindowTitle = true,
                Title = "Melody of Memory Re:Chart",
                Icon = "/wwwroot/icon.ico",
                Width = 1920,
                Height = 1080
            };

            var window = await Electron.WindowManager.CreateWindowAsync(options);
            window.Maximize();
            window.OnClosed += () => {
                Electron.App.Quit();
            };
        }
    }
}