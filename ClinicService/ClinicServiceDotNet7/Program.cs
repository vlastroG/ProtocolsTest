using ClinicService.Data.DbContexts;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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


            #region Congifure Swagger

            builder.Services.AddGrpcSwagger();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "ClinicService", Version = "v1" });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "ClinicServiceDotNet7.xml");
                c.IncludeXmlComments(filePath);
                c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
            });

            #endregion



            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

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