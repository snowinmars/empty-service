using Autofac.Extensions.DependencyInjection;
using EmptyService.CommonEntities.Pathes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EmptyService.WebApi
{
    // ReSharper disable once AllowPublicClass
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(x => x.UseStartup<Startup>()
                                                       .UseContentRoot(DirectoryPath.Current))
                       .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                       .UseSerilog();
        }
    }
}