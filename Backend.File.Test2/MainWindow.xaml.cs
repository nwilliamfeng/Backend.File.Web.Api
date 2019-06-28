using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Text;
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
using Microcomm;
using Microsoft.Win32;

namespace Backend.File.Test2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string uploadurl = System.Configuration.ConfigurationManager.AppSettings["uploadUrl"];
        private static string formPath = "/api/file/UploadWithForm";
        private static string uppath = "/api/file/Upload";
       private static string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1NjIwMzE2MzksInVzZXJJZCI6ImZ3IiwidGltZXN0YW1wIjoxNTYxNDI2ODM5fQ.MmYK1RTIwXOP4HPiAY9NsYfWvkRQ9UwVPtr_KqsZ2CA";
        //  private static string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1NjIxNjg5NzAsInVzZXJJZCI6ImZ3IiwidGltZXN0YW1wIjoxNTYxNTY0MTcwfQ.sZbItuQcg9X6nKWeHJgInozLRNA2woTRYQExzDZbawg";

        private HttpClientUtil _uploadhttpClient  = new HttpClientUtil(uploadurl);

        private HttpClientUtil _downloadClient = new HttpClientUtil("http://127.0.0.1:5700/");

        public MainWindow()
        {
             InitializeComponent();


            //https://stackoverflow.com/questions/14597232/asp-net-web-api-client-progressmessagehandler-post-task-stuck-in-winform-app
            //https://stackoverflow.com/questions/20661652/progress-bar-with-httpclient


            _uploadhttpClient.HttpSendProgress += (s, arg) =>
            {
                Console.WriteLine("send: " + arg.ProgressPercentage);
                this.Dispatcher.Invoke(() => this.sendProgressBar.Value = arg.ProgressPercentage);
            };

            _downloadClient.HttpReceiveProgress += (s, arg) =>
            {
                Console.WriteLine("receive: " + arg.ProgressPercentage);
                this.Dispatcher.Invoke(() => this.downLoadProgressBar.Value = arg.ProgressPercentage);
            };
        }

       

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.sendProgressBar.Value = 0;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var dr = openFileDialog.ShowDialog();
            if (dr == false)
                return;
           
            var name = openFileDialog.FileName;
           
            var result = await this._uploadhttpClient.Upload(formPath, new string[] { name }, new Dictionary<string, string>() { ["Authorization"] = token, ["category"] = "newDir" });
            this.infoTextBox.Text = result;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           
        }



        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.downLoadUrlTextBox.Text))
                return;
           

            var result = await _downloadClient.Download(this.downLoadUrlTextBox.Text );
            System.IO.File.WriteAllBytes("d:\\aaa"+"."+this.downLoadUrlTextBox.Text.Split('.').LastOrDefault(),result);
        }
    }
}
