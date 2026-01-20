using PocLoginWebsite.ConsoleApp.Services;
using PocLoginWebsite.Infrastructure.Configuration;

namespace PocLoginWebsite.ConsoleApp;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var config = new AppConfiguration();
        var logger = new ConsoleLogger();
        var pocService = new SauceDemoPocService(logger, config);

        return await pocService.RunAsync();
    }
}
