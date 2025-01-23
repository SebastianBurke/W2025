using System.Text;
using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using HIAAAServices.Controllers;
using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DAL.Services;
using HIAAAServices.DAL.Repositories;
using HIAAAServices.Models;
using Microsoft.EntityFrameworkCore;

namespace HIAAAServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddServiceModelServices().AddServiceModelMetadata();
            builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddDbContext<Hia3Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

            // Register ServiceDebugBehavior for detailed exceptions
            builder.Services.AddSingleton<IServiceBehavior>(new ServiceDebugBehavior
            {
                IncludeExceptionDetailInFaults = true
            });

            builder.Services.AddControllers();

            // Add Swagger/OpenAPI services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var conString = builder.Configuration.GetConnectionString("MyConnection");
            builder.Services.AddDbContext<Hia3Context>(options => options.UseSqlServer(conString));
            builder.Services.AddScoped<IRole, RoleRepository>();
            builder.Services.AddScoped<IAppAdminRepository, AppAdminRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
            builder.Services.AddScoped<IApp, AppRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            var app = builder.Build();

            // Define bindings
            var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None)
            {
                MaxReceivedMessageSize = int.MaxValue,
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max
            };

            var soap12Binding = new CustomBinding(
                new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8),
                new HttpTransportBindingElement
                {
                    MaxReceivedMessageSize = int.MaxValue
                });

            // Configure ServiceMetadataBehavior for WSDL
            var metadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
            metadataBehavior.HttpGetEnabled = true;
            metadataBehavior.HttpGetUrl = new Uri("http://localhost:5264/SOAP/LoginService?wsdl");
            metadataBehavior.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

            // Middleware for logging request paths
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Request Path: {context.Request.Path}");
                await next();
            });

            // Middleware to log raw SOAP requests
            app.Use(async (context, next) =>
            {
                if (!context.Request.Body.CanSeek)
                {
                    var buffer = new MemoryStream();
                    await context.Request.Body.CopyToAsync(buffer);
                    buffer.Position = 0;
                    context.Request.Body = buffer;
                }
                else
                {
                    context.Request.Body.Position = 0;
                }

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    var rawRequest = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                    Console.WriteLine($"Raw SOAP Request: {rawRequest}");
                }

                await next();
            });

            // Add ServiceModel middleware for SOAP endpoints
            app.UseServiceModel(builder =>
            {
                builder.AddService<LoginService>();
                builder.AddServiceEndpoint<LoginService, ILoginService>(basicHttpBinding, "/SOAP/LoginService");
                builder.AddServiceEndpoint<LoginService, ILoginService>(soap12Binding, "/SOAP12/LoginService");
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
