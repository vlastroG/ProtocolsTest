using ClinicService.Data.DbContexts;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ClinicServiceDotNet7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region Configure Kestrel
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 5100, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
                options.Listen(IPAddress.Any, 5101, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1;
                });
            });
            #endregion

            #region Configuration gRPC

            builder.Services.AddGrpc()
                .AddJsonTranscoding();

            #endregion


            #region Configure EF DB Context Service (ClinicService Database)
            builder.Services.AddDbContext<ClinicServiceDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });
            #endregion



            // Add services to the container.
            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseRouting();
            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

            app.MapGrpcService<Services.Impls.ClinicService>().EnableGrpcWeb();
            app.MapGet("/",
                () =>
                "Communication with gRPC endpoints must be made through a gRPC client.");

            app.Run();
        }
    }
}