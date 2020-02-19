using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace OpenFM_Results_Viewer
{
    public partial class MainView : Form
    {
        private static string _fileName => "openfm_channels.json";
        private static string _saveDirectory;

        public MainView()
        {
            InitializeComponent();
            _saveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            treeView.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;

            var channelsList = ReadFromLocal();
            SortList(channelsList);
            RefreshTree(channelsList);
        }

        private void SortList(List<SharedModels.Models.Saved.Channel> channelsList)
        {
            channelsList.Sort((x, y) => { return x.Name.CompareTo(y.Name); });

            foreach(var channel in channelsList)
                channel.Songs.Sort((x, y) => { return x.Artist.CompareTo(y.Artist); });
            
        }

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var youtubeUrl = "https://www.youtube.com/results?search_query=";

            var song = e.Node.Tag as SharedModels.Models.Saved.Song;
            var query = song.Artist + " " + song.Name;
            
            var encodedQuery = System.Net.WebUtility.UrlEncode(query);
            System.Diagnostics.Process.Start(youtubeUrl + encodedQuery);
        }

        private void RefreshTree(List<SharedModels.Models.Saved.Channel> channelsList)
        {
            treeView.Nodes.Clear();
            var date = DateTime.Now;

            foreach(var channel in channelsList)
            {
                var node = treeView.Nodes.Add(channel.Id.ToString(), channel.Name);
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

        private List<SharedModels.Models.Saved.Channel> ReadFromLocal()
        {
            var fullPath = _saveDirectory + "/" + _fileName;
            var list = new List<SharedModels.Models.Saved.Channel>();

            if (!File.Exists(fullPath))
                return list;

            list = JsonConvert.DeserializeObject<List<SharedModels.Models.Saved.Channel>>(File.ReadAllText(fullPath));
            return list;
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
    }
}
