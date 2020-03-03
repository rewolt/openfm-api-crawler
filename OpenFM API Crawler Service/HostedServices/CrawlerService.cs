using Microsoft.Extensions.Hosting;
using OpenFM_API_Crawler_Service.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenFM_API_Crawler_Service.HostedServices
{
    public class CrawlerService : BackgroundService
    {
        private readonly ApiRepository _apiRepository;
        private readonly ILogger _logger;
        

        public CrawlerService(ApiRepository apiRepository, ILogger logger)
        {
            _apiRepository = apiRepository;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Debug($"{nameof(CrawlerService)} is starting.");
            stoppingToken.Register(() => _logger.Debug($"{nameof(CrawlerService)} is stopping."));
            var delayTime = TimeSpan.FromSeconds(5);


            while(!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(delayTime);
                _logger.Information($"{delayTime.TotalMilliseconds}ms passed away...");
                var assets = await _apiRepository.GetChannelsData();
                _logger.Information($"{assets.Channels.Count} channels downloaded.");
            }

            _logger.Debug($"{nameof(CrawlerService)} is stopped.");
        }
    }
}
