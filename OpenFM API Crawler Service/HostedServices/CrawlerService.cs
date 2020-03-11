using Microsoft.Extensions.Hosting;
using OpenFM_API_Crawler_Service.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedModels.Models.ApiSongs;

namespace OpenFM_API_Crawler_Service.HostedServices
{
    public class CrawlerService : BackgroundService
    {
        private readonly ApiRepository _apiRepository;
        private readonly ILogger _logger;
        private readonly LocalRepository _localRepository;

        public CrawlerService(ApiRepository apiRepository, ILogger logger, LocalRepository localRepository)
        {
            _apiRepository = apiRepository;
            _logger = logger;
            _localRepository = localRepository;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Debug($"{nameof(CrawlerService)} is starting.");
            stoppingToken.Register(() => _logger.Debug($"{nameof(CrawlerService)} is stopping."));
            var delayTime = TimeSpan.FromSeconds(5);


            while(!stoppingToken.IsCancellationRequested)
            {
                var getChannelsWithSongsTask = GetChannelsData();
                var getChannelsTask = GetChannels();
                var tasks = new Task[] { getChannelsWithSongsTask, getChannelsTask };
                await Task.WhenAll(tasks);

                if (getChannelsTask.Result is null || getChannelsWithSongsTask.Result is null)
                    continue;

                var saveChannelsTask = SaveChannels(getChannelsTask.Result);
                var saveSongsTask = SaveSongs(getChannelsWithSongsTask.Result);
                tasks = new Task[] { saveChannelsTask, saveSongsTask };
                await Task.WhenAll(tasks);

                stoppingToken.ThrowIfCancellationRequested();
                await Task.Delay(delayTime, stoppingToken);
            }

            _logger.Debug($"{nameof(CrawlerService)} is stopped.");
        }


        private async Task<List<SharedModels.Models.ApiSongs.Channel>> GetChannelsData()
        {
            _logger.Information("Collecting channels list from OpenFM API...");
            var retries = 3;
            var delayTime = TimeSpan.FromSeconds(2);

            while(retries > 0)
            {
                try
                {
                    var assets = await _apiRepository.GetChannelsData();
                    var channelsData = assets.Channels;
                    _logger.Information($"{channelsData.Count} channels collected.");
                    return channelsData;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString(), $"Cannot download channels data (with songs). Retries left: {--retries}. Retrying...");
                    await Task.Delay(delayTime);
                    delayTime += TimeSpan.FromSeconds(1);
                }
            }

            _logger.Warning("Cannot download channels for a long time. Maybe not this time...");
            return null;
        }

        private async Task<List<SharedModels.Models.ApiChannels.Channel>> GetChannels()
        {
            _logger.Information("Collecting songs list from OpenFM API...");
            var retries = 3;
            var delayTime = TimeSpan.FromSeconds(3);

            while(retries > 0)
            {
                try
                {
                    var channelsList = await _apiRepository.GetChannelsList();
                    _logger.Information("Channels data (with songs) collected.");
                    return channelsList.Channels;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.ToString(), $"Cannot download channels. Retries left: {--retries}. Retrying...");
                    await Task.Delay(delayTime);
                    delayTime += TimeSpan.FromSeconds(1);
                }
            }

            _logger.Warning("Cannot download channels data (with songs) for a long time. Maybe not this time...");
            return null;
        }

        private Task SaveChannels(IEnumerable<SharedModels.Models.ApiChannels.Channel> channels)
        {
            return Task.Run(() =>
            {
                foreach (var channel in channels)
                    _localRepository.UpsertChannel(new SharedModels.Models.DTO.Channel { Name = channel.Name });
            });
        }

        private Task SaveSongs(List<Channel> channelsWithSongs)
        {
            return Task.Run(() =>
            {
                foreach (var channelWithSongs in channelsWithSongs)
                {
                    foreach (var song in channelWithSongs.Tracks)
                    {
                        var songToSave = new SharedModels.Models.DTO.Song
                        {
                            Album = song.Song.Album?.Title ?? string.Empty,
                            Artist = song.Song.Artist,
                            Name = song.Song.Title,
                            OpenfmChannelId = channelWithSongs.Id
                        };

                        _localRepository.UpsertSong(songToSave);
                    }
                }
            });
        }
    }
}
