using OpenFM_Youtube_Downloader.Services;
using System;

namespace OpenFM_Youtube_Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting OpenFM Youtube Downloader");
            var mainService = new MainService();
            mainService.Execute();
            Console.WriteLine("End program...");
        }
    }
}
