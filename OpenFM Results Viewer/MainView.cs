using Newtonsoft.Json;
using OpenFM_Results_Viewer.Models.SavedObjects;
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

        private void SortList(List<Channel> channelsList)
        {
            channelsList.Sort((x, y) => { return x.Name.CompareTo(y.Name); });

            foreach(var channel in channelsList)
                channel.Songs.Sort((x, y) => { return x.Artist.CompareTo(y.Artist); });
            
        }

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var youtubeUrl = "https://www.youtube.com/results?search_query=";

            var artist = e.Node.Text.Substring(1, e.Node.Text.IndexOf("] ") -1);
            var title = e.Node.Text.Substring(e.Node.Text.LastIndexOf(") ") + 2);

            var artistTitle = artist + " " + title;
            artistTitle = System.Net.WebUtility.UrlEncode(artistTitle);
            System.Diagnostics.Process.Start(youtubeUrl + artistTitle);
        }

        private void RefreshTree(List<Channel> channelsList)
        {
            treeView.Nodes.Clear();

            foreach(var channel in channelsList)
            {
                var node = treeView.Nodes.Add(channel.Id.ToString(), channel.Name);
                foreach (var song in channel.Songs)
                {
                    var childNode = node.Nodes.Add($"[{song.Artist}] ({song.Album}) {song.Name}");
                }
            }
        }

        private List<Models.SavedObjects.Channel> ReadFromLocal()
        {
            var fullPath = _saveDirectory + "/" + _fileName;
            var list = new List<Models.SavedObjects.Channel>();

            if (!File.Exists(fullPath))
                return list;

            list = JsonConvert.DeserializeObject<List<Models.SavedObjects.Channel>>(File.ReadAllText(fullPath));
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
