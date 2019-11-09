using System;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Core;
using EmptyService.CommonEntities.Exceptions;
using EmptyService.CommonEntities.Pathes;
using EmptyService.Configuration;
using EmptyService.Configuration.Abstractions;
using EmptyService.DependencyResolver.ConfigurationModels;
using EmptyService.Logger;
using EmptyService.Logger.Abstractions;
using Newtonsoft.Json;

namespace EmptyService.DependencyResolver
{
    // ReSharper disable once AllowPublicClass
    public static class Resolver
    {
        private static readonly string ConfigFileName = "connection-strings.json";

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static void Register()
        {
            var builder = new ContainerBuilder();

            Register(builder);
        }

        public static void Register(ContainerBuilder builder)
        {
            var (config, logger) = Init();

            logger.Information("Resolving dependencies");

            BuildContainer(builder, logger, config);
        }

        private static void BuildContainer(ContainerBuilder builder, ILog log, IMainConfig config)
        {
            builder.RegisterInstance(log).As<ILog>().SingleInstance();

            builder.RegisterInstance(config).As<IMainConfig>().SingleInstance();
            builder.RegisterInstance(config.MyDatabase).As<IDatabaseConfig>().SingleInstance();
            builder.RegisterInstance(config.Log).As<ILogConfig>().SingleInstance();
        }

        public static void Validate(ILifetimeScope container, ILog log)
        {
            ValidateResolvingSchema(container, log);
        }

        private static void ValidateResolvingSchema(ILifetimeScope container, ILog log)
        {
            var services = container
                           .ComponentRegistry
                           .Registrations
                           .SelectMany(x => x.Services)
                           .OfType<TypedService>()
                           .ToArray();

            foreach (var service in services.Where(x => !x.ServiceType.FullName.Contains("Actor")))
            {
                var logString = $"Resolving {service.Description} >";
                log.Debug(logString);
                var item = container.Resolve(service.ServiceType);
                log.Debug($"{new string(' ', logString.Length - service.Description.Length)} {item.GetType().FullName}");
            }

            log.Debug("DI schema is valid");
        }

        private static (IMainConfig config, ILog logger) Init()
        {
            IMainConfig config;

            try
            {
                config = ReadConfig();
            }
            catch (Exception e)
            {
                throw new InitializationException("Can't read configuration entity", e);
            }

            ILog log;

            try
            {
                log = new Log(config.Log.LogFilePath, config.Log.Level);
                log.Information($"Working in {Directory.GetCurrentDirectory()}");
                log.Information($"Log settings: '{log.LogPath} / {log.LogLevel}'");
            }
            catch (Exception e)
            {
                throw new InitializationException("Can't create logger", e);
            }

            return (config, log);
        }

        private static IMainConfig ReadConfig()
        {
            var currentDirectory = DirectoryPath.Current;

            var configText =
                currentDirectory.FindChildFile(ConfigFileName, ActionOnNotFound.ThrowNewException);

            var config = JsonConvert.DeserializeObject<MainConfigModel>(File.ReadAllText(configText))
                                    .ToMainConfig(currentDirectory);

            return config;
        }

        private static MainConfig ToMainConfig(this MainConfigModel model, DirectoryPath currentDirectory)
        {
            var myDatabase = new DatabaseConfig(new Uri(model.MyDatabase.Host),
                                                (int)uint.Parse(model.MyDatabase.Port),
                                                model.MyDatabase.Username,
                                                model.MyDatabase.Password,
                                                model.MyDatabase.DatabaseName);

            var log = new LogConfig(currentDirectory.CombineFile(model.Log.LogFilePath),
                                    model.Log.Level);

            return new MainConfig(myDatabase, log);
        }

        private static IContainer Container { get; set; }
    }
}