# openfm-api-crawler
Simple open.fm API client for listing songs from channels and visualizer with fast youtube search.

There are two projects:
- OpenFM API Crawler - .NET Core 2.0 project
- OpenFM Results Viewer - .NET Framework 4.7.1 project

Data from research will be stored in "openfm_channels.json" in user Documents folder.
Songs Reader just reads result json. Double click on song will open default web browser with best youtube matches.
OpenFM API returns only a few nearest songs that will be played per channel. Crawler will add new songs to actual json repository if song not existed there before. Crawler runs once and exit so for best results you need to add task in windows task scheduler and run Crawler every 5 min. 

Crawler needs to be compiled using dotnet CLI. For fast way just download project, run cmd/terminal in solution folder (where OpenFmCrawler.sln is)
and type <i>dotnet build -c Release -r win10-x64 "OpenFM API Crawler\OpenFM API Crawler.csproj"</i> to build for Windows 10.
