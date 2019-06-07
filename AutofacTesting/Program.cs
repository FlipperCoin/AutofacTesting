using Autofac;
using Autofac.Configuration;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Configuration;
using System;

namespace AutofacTesting
{
    /*
     * TODO:
     * 1. Use interceptors - 
     *          Need to mention enable interface interceptors or class interceptors because the two have different limitations.
     *          interface interceptors require your component to be exposed as a service with and interface, not a class
     *          class interceptors require you to virtualize the intercepted methods
     * 2. Containerize app to demonstrate autofac containerized
     *          Works! :D
     * 3. Override components with configuration
     *          All good :)
     * 4. Configuration with yaml and hjson
     */
    class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            var config = new ConfigurationBuilder();

            containerBuilder.RegisterType<DependencyA>().As<IDependency>();
            containerBuilder.RegisterType<AppInterceptor>();
            containerBuilder.RegisterType<App>()
                .As<IApp>()
                .InterceptedBy(typeof(AppInterceptor))
                .EnableInterfaceInterceptors();

            var is_config_optional = true;
            config.AddJsonFile("autofac_config.json", is_config_optional);
            var config_module = new ConfigurationModule(config.Build());
            containerBuilder.RegisterModule(config_module);

            var container = containerBuilder.Build();

            var app = container.Resolve<IApp>();
            app.Run();
        }
    }
}
