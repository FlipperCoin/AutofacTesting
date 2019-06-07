using Autofac;
using Autofac.Configuration;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NodeDeserializers;

namespace AutofacTesting
{
    /*
     * 1. Use interceptors - 
     *          Need to mention enable interface interceptors or class interceptors because the two have different limitations.
     *          interface interceptors require your component to be exposed as a service with and interface, not a class
     *          class interceptors require you to virtualize the intercepted methods
     * 2. Containerize app to demonstrate autofac containerized
     *          Works! :D
     * 3. Override components with configuration
     *          All good :)
     * 4. Configuration with yaml
     *          Can use NuGet that works with .NET's ConfigurationBuilder,
     *          that way the app can stay compatible with other configuration formats in the future
     *          and we can expect the best features from a widely used configuration framework
     */
    class Program
    {
        static void Main(string[] args)
        {
            #region App configuration

            var configFile = "config.yml";
            ValidateConfigFile(configFile);
            IConfigurationRoot config = GetConfig(configFile);
            var inputs = config.GetSection("input").Get<IEnumerable<InputMetadata>>();

            #endregion

            #region IoC Container

            var containerBuilder = new ContainerBuilder();
            var containerConfig = new ConfigurationBuilder();

            containerBuilder.RegisterType<DependencyA>().As<IDependency>();
            containerBuilder.RegisterType<AppInterceptor>();
            containerBuilder.RegisterType<App>()
                .As<IApp>()
                .WithParameters(new Parameter[] {
                    new NamedParameter("inputs", inputs)
                })
                .InterceptedBy(typeof(AppInterceptor))
                .EnableInterfaceInterceptors();

            var isContainerConfigOptional = true;
            containerConfig.AddJsonFile("autofac_config.json", isContainerConfigOptional);
            var configModule = new ConfigurationModule(containerConfig.Build());
            containerBuilder.RegisterModule(configModule);

            var container = containerBuilder.Build();

            #endregion

            #region App Resolution & Run

            var app = container.Resolve<IApp>();
            app.Run();

            #endregion
        }

        private static void ValidateConfigFile(string configFile)
        {
            if (!File.Exists(configFile))
            {
                Console.WriteLine($"Couldn't find the configuration file. Path: {configFile}");
                return;
            }

            var deserializer = new DeserializerBuilder()
                .WithNodeDeserializer(inner => new ValidatingNodeDeserializer(inner), s => s.InsteadOf<ObjectNodeDeserializer>())
                .Build();
            using (var streamReader = new StreamReader(configFile))
            {
                try
                {
                    deserializer.Deserialize(streamReader);
                }
                catch(SemanticErrorException e)
                {
                    Console.WriteLine(e.InnerException);
                }

            } 
        }

        private static IConfigurationRoot GetConfig(string configFile)
        {
            var isConfigOptional = true;
            return new ConfigurationBuilder().AddYamlFile("config.yml", isConfigOptional).Build();
        }
    }
}
