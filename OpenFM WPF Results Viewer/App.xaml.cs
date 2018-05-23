using OpenFM_WPF.ViewModels;
using System.Windows;

namespace OpenFM_WPF
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            TreeViewModel treeViewModel = new TreeViewModel();
            window.DataContext = treeViewModel;
            window.Show();
        }
    }
}
