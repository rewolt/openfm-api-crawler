using Newtonsoft.Json;
using SharedModels.Models.DTO;
using OpenFM_Results_Viewer.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using SharedModels.Models.Saved;

namespace OpenFM_Results_Viewer
{
    public partial class MainView : Form
    {
        private static string _fileName => "songs.bin";
        private static string _saveDirectory;

        public MainView()
        {
            InitializeComponent();
            //_saveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _saveDirectory = @"C:\Workspace\source\repos\openfm-api-crawler\OpenFM API Crawler Service\bin\Debug\netcoreapp3.1\Database";
            treeView.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;

            var channelsList = ReadFromLocal();
            SortList(channelsList);
            RefreshTree(channelsList);
        }

        private void SortList(List<ViewModel.Channel> channelsList)
        {
            channelsList.Sort((x, y) => { return x.Name.CompareTo(y.Name); });

            foreach(var channel in channelsList)
                channel.Songs.Sort((x, y) => { return x.Artist.CompareTo(y.Artist); });
            
        }

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var youtubeUrl = "https://www.youtube.com/results?search_query=";

            var song = e.Node.Tag as ViewModel.Song;
            var query = song.Artist + " " + song.Name;
            
            var encodedQuery = System.Net.WebUtility.UrlEncode(query);
            System.Diagnostics.Process.Start(youtubeUrl + encodedQuery);
        }

        private void RefreshTree(List<ViewModel.Channel> channelsList)
        {
            treeView.Nodes.Clear();
            var date = DateTime.UtcNow;

            foreach(var channel in channelsList)
            {
                var node = treeView.Nodes.Add(channel.GetHashCode().ToString(), channel.Name);
                foreach (var song in channel.Songs)
                {
                    var childNode = node.Nodes.Add($"[{song.Artist}] ({song.Album}) {song.Name}");
                    childNode.Tag = song;
                    if (song.CreatedAt > new DateTime(date.Year, date.Month, date.Day))
                    {
                        node.BackColor = System.Drawing.Color.Yellow;
                        childNode.BackColor = System.Drawing.Color.Yellow;
                    }
                }
            }
        }

        //private List<Channel> ReadFromLocal()
        //{
        //    var fullPath = _saveDirectory + "/" + _fileName;
        //    var list = new List<Channel>();

        //    if (!File.Exists(fullPath))
        //        return list;

        //    list = JsonConvert.DeserializeObject<List<Channel>>(File.ReadAllText(fullPath));
        //    return list;
        //}

        private List<ViewModel.Channel> ReadFromLocal()
        {
            var fullPath = Path.Combine(_saveDirectory, _fileName);
            var list = new List<ViewModel.Channel>();

            if (!File.Exists(fullPath))
                return list;

            else
            {
                var data = ReadSavedData();
                foreach (var channel in data._channels)
                    list.Add(new ViewModel.Channel
                    {
                        Name = channel.Name,
                        Songs = data._songs
                                    .Where(x => x.OpenfmChannelIds.Contains(channel.Id))
                                    .Select(x => new ViewModel.Song { Album = x.Album, Artist = x.Artist, Name = x.Name, CreatedAt = x.CreatedAt, LastSeenAt = x.LastSeenAt })
                                    .ToList()
                    });
                return list;
            }
        }

        private void bt_expand_Click(object sender, EventArgs e)
        {
            treeView.ExpandAll();
        }

        private void bt_colapse_Click(object sender, EventArgs e)
        {
            treeView.CollapseAll();
        }

        private void bt_refresh_Click(object sender, EventArgs e)
        {
            var channelsList = ReadFromLocal();
            SortList(channelsList);
            RefreshTree(channelsList);
        }

        private Data ReadSavedData()
        {
            var fullPath = Path.Combine(_saveDirectory, _fileName);

            try
            {
                if (!File.Exists(fullPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                    File.WriteAllText(fullPath, string.Empty);
                }

                using (var fileStream = File.OpenRead(fullPath))
                {
                    if (fileStream.Length == 0)
                        return new Data();

                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var data = formatter.Deserialize(fileStream);
                    if (data is null)
                        return new Data();
                    else
                        return (Data)data;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
