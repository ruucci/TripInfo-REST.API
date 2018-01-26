using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using TripInfoREST.API.Entities;
using TripInfoREST.API.Helpers;
using TripInfoREST.API.Services;

namespace TripInfoREST.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public static IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());

            });
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif

            // register the DbContext on the container, getting the connection string from
            // appSettings (note: use this during development; in a production environment,
            // it's better to store the connection string in an environment variable)

            var connectionString = Configuration["connectionStrings:tripDBConnectionString"];
            services.AddDbContext<TripContext>(o => o.UseSqlServer(connectionString));

            // register the repository
            services.AddScoped<ITripInfoRepository, TripInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TripContext tripContext)
        {
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500,
                                exceptionHandlerFeature.Error,
                                exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Oops! Something went wrong. Try again later.");

                    });
                });
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Destination, Models.DestinationDto>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                                                                     $"{src.Name}, {src.State}"))
                    .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                                                                    src.DateOfVisit.GetCurrentAge()));

                cfg.CreateMap<Entities.Attraction, Models.AttractionDto>();

                cfg.CreateMap<Models.DestinationForCreationDto, Entities.Destination>();

                cfg.CreateMap<Models.AttractionForCreationDto, Entities.Attraction>();

                cfg.CreateMap<Models.AttractionForUpdateDto, Entities.Attraction>();

                cfg.CreateMap<Entities.Attraction, Models.AttractionForUpdateDto>();
            });

            //tripContext.EnsureSeedDataForContext();

            app.UseMvc();
        }
    }
}
