using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalRGame;
using SignalRGame.Hubs;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddRazorPages();
//builder.Services.AddSignalR();

//builder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "../reactProject1/build";
//});

////needed?
////builder.Services.AddSingleton<IHubContext<ChatHub, IChatClient>>();
//builder.Services.AddSingleton<GameService>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

////app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<ChatHub>("/chatHub");
//    endpoints.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
//       string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
//    endpoints.MapRazorPages();
//});

//app.UseSpa(spa =>
//{
//    spa.Options.SourcePath = "../reactProject1";


//    if (app.Environment.IsDevelopment())
//    {
//        spa.UseReactDevelopmentServer(npmScript: "start");
//    }
//});

//app.Run();


namespace SignalRGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}