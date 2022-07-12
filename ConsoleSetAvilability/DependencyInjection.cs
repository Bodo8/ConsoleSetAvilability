using ConsoleSetAvilability.Library;
using ConsoleSetAvilability.Library.Commons;
using ConsoleSetAvilability.Library.Helpers;
using ConsoleSetAvilability.Library.Helpers.Interfaces;
using ConsoleSetAvilability.Library.Infrastracture.ByEmail;
using ConsoleSetAvilability.Library.Infrastracture.Interfaces;
using ConsoleSetAvilability.Library.Logic;
using ConsoleSetAvilability.Library.Logic.Interfaces;
using ConsoleSetAvilability.Library.Models.ConfigurationModels;
using ConsoleSetAvilability.Library.Services;
using ConsoleSetAvilability.Library.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AktualizatorProductManag2Click
{
    public class DependencyInjection
    {
        public static IServiceProvider ConfigureServices()
        {
            var config = new ConfigurationBuilder()
                             .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                             .Build();

            MapperConfiguration mapperConfig = GetMapperConfig();
            IMapper mapper = mapperConfig.CreateMapper();

            var serviceCollection = new ServiceCollection()
                                       .AddLogging(loggingBuilder =>
                                       {
                                           loggingBuilder.ClearProviders();
                                           loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                                           loggingBuilder.AddNLog(config);
                                       })
                                       .AddSingleton<IConfiguration>(config)
                                       .AddSingleton(mapper)
                                       .AddSingleton<IMainClass, MainClass>()
                                       .AddTransient<IDateTimeHelper, DateTimeHelper>()
                                       .AddTransient<IWebProcessor, WebProcessor>()
                                       .AddTransient<IWebService, WebService>()
                                       .AddTransient<ISourceService, SourceService>()
                                       .AddTransient<IUpdateProductsByApiService, UpdateProductsByApiService>()
                                       .AddTransient<ITwoClicApiService, TwoClicApiService>()
                                       .AddTransient<IProductSettingsProcessor, ProductSettingsProcessor>()
                                       .AddTransient<INotificationsService, NotificationsService>()
                                       .AddTransient<IConverterEmailsDto, ConverterEmailsDto>()
                                       .AddTransient<IEmailComposer, EmailComposer>()
                                       .AddTransient<IEmailNotifications, EmailNotifications>()
                                       .AddTransient<IEmailProcessor, EmailProcessor>()
                                       .AddTransient<IWebShopService, WebShopService>()
                                       .AddTransient<ICheckboxesProductService, CheckboxesProductService>()
                                       ;

            NLog.LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
            serviceCollection.Configure<AppSettings>(options => config.GetSection("AppSettings").Bind(options));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        private static MapperConfiguration GetMapperConfig()
        {
            return new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
        }
    }
}
