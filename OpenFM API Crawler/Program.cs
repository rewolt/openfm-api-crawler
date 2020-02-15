using Microsoft.Extensions.Configuration;
using OpenFM_API_Crawler.Services;

namespace OpenFM_API_Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainService = new MainService();
            mainService.Execute().Wait();
        }
    }
}
