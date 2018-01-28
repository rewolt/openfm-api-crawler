using System;

namespace OpenFM_API_Crawler
{
    class Program
    {
        private static string _apiUrl => "http://open.fm/api/api-ext/v2/channels/long.json?_=1517168611.79";

        //// Browser header to API
        //GET /api/api-ext/v2/channels/long.json? _ = 1517168611.79 HTTP/1.1
        // Host: open.fm
        // Connection: keep-alive
        // Accept: */*
        // X-Requested-With: XMLHttpRequest
        // User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Safari/537.36
        // Referer: http://open.fm/play/82
        // Accept-Encoding: gzip, deflate
        // Accept-Language: pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7
        // Cookie: PWA_adbd=1; pvid=f29de175c69358e6ce40

        static void Main(string[] args)
        {
            if (Uri.IsWellFormedUriString(_apiUrl, UriKind.Absolute))
                Console.WriteLine("SUper uri");
            var uri = new Uri(_apiUrl);

            var downloader = new JsonDownloader(uri);
            Console.WriteLine(downloader.GetJson());
            Console.ReadKey();
        }
    }
}
