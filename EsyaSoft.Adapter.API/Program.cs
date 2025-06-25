
using EsyaSoft.Adapter.API.BasicAuth;
using EsyaSoft.Adapter.API.EFModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;


namespace EsyaSoft.Adapter.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Hangfire

            //builder.Services.AddHangfire(x => 
            //x.UseSqlServerStorage(@"Data Source=DESKTOP-8RL8JOG;Initial 
            //    Catalog=hangfire;Integrated Security=True;Pooling=False"));
            builder.Services.AddHangfire(x => 
            x.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"))
            .WithJobExpirationTimeout(TimeSpan.FromHours(6)));
            //builder.Services.AddHangfire(hangfire =>
            //{
            //    hangfire.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
            //    hangfire.UseSimpleAssemblyNameTypeSerializer();
            //    hangfire.UseRecommendedSerializerSettings();
            //    hangfire.UseColouredConsoleLogProvider();
            //    hangfire.UseSqlServerStorage(
            //                 builder.Configuration.GetConnectionString("HangfireConnection"),
            //        new SqlServerStorageOptions
            //        {
            //            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //            QueuePollInterval = TimeSpan.Zero,
            //            UseRecommendedIsolationLevel = true,
            //            DisableGlobalLocks = true
            //        });

            //    var server = new BackgroundJobServer(new BackgroundJobServerOptions
            //    {
            //        ServerName = "hangfire-test",
            //    });
            //});
            builder.Services.AddHangfireServer();

            //SOAP 
            builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();
            builder.Services.AddHttpContextAccessor();
            //Serilog
            var ls = new LoggingLevelSwitch();
            var logger = new LoggerConfiguration().ReadFrom
                        .Configuration(builder.Configuration)
                        .MinimumLevel.ControlledBy(ls)
                        //.Enrich.FromLogContext()
                        .Enrich.WithClientIp(headerName: "CF-Connecting-IP")
                        .Enrich.WithCorrelationId(headerName: "correlation-id", addValueIfHeaderAbsence: true)
                        //.Enrich.WithRequestHeader("Header-Name1")
                        .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MDM Adapter API", Version = "v1" });
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,  
                    Description = "Basic Authorization header."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            //Authorization
            builder.Services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            //Register SQL Server DB
            builder.Services.AddDbContext<AdapterContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AdapterDBCon"));
            });

            var app = builder.Build();
            
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHangfireDashboard();
            });
            //app.UseRouting();
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new BasicAuthAuthorizationFilter(
                    new BasicAuthAuthorizationFilterOptions
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = "AEML",
                                PasswordClear = "Adapter"

                            }
                        }
                    }) }
            });

            app.StartRecurringJobs();
            //}
            //app.MapControllers();
            app.Run();
        }
    }
}
