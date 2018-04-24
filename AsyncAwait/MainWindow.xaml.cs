using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await DownloadAsync();            
        }             

        public async Task DownloadAsync()
        {
            var reg = (HttpWebRequest)WebRequest.Create("http://microsoft.com");
            reg.Method = "GET";
            var task = reg.GetResponseAsync();
            //Task<WebResponse> task = Task.Factory.FromAsync<WebResponse>(
            //   reg.BeginGetResponse, reg.EndGetResponse, null);
            var resp = (HttpWebResponse) await task;            
            TextBox1.Text += (resp.Headers.ToString() + "Async completed");            
        }
    }
}
