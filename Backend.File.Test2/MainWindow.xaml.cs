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
        //  private static string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1NjIwMzE2MzksInVzZXJJZCI6ImZ3IiwidGltZXN0YW1wIjoxNTYxNDI2ODM5fQ.MmYK1RTIwXOP4HPiAY9NsYfWvkRQ9UwVPtr_KqsZ2CA";
        private static string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1NjIxNjg5NzAsInVzZXJJZCI6ImZ3IiwidGltZXN0YW1wIjoxNTYxNTY0MTcwfQ.sZbItuQcg9X6nKWeHJgInozLRNA2woTRYQExzDZbawg";

        public MainWindow()
        {
             InitializeComponent();

           

            //https://stackoverflow.com/questions/14597232/asp-net-web-api-client-progressmessagehandler-post-task-stuck-in-winform-app
            //https://stackoverflow.com/questions/20661652/progress-bar-with-httpclient
        }

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    this.progressBar.Value = 0;
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    var dr =openFileDialog.ShowDialog();
        //    if (dr == false)
        //        return;
        //    ProgressMessageHandler progressHandler = new ProgressMessageHandler();
        //    progressHandler.HttpSendProgress += (s, arg) =>
        //    {
        //        Console.WriteLine("send: " + arg.ProgressPercentage);
        //        this.Dispatcher.Invoke(() => this.progressBar.Value = arg.ProgressPercentage);
        //    };

        //    progressHandler.HttpReceiveProgress += (s, arg) =>
        //    {
        //        Console.WriteLine("receive: " + arg.ProgressPercentage);
        //      //  this.Dispatcher.Invoke(() => this.progressBar.Value = arg.ProgressPercentage);
        //    };
        //    var name = openFileDialog.FileName;
        //   var result =await new HttpClientUtil2(uploadurl).Upload(formPath,new string[] { name},new Dictionary<string, string>() {["Authorization"]=token,["dir"]="newDir" },null, progressHandler);
        //    this.infoTextBox.Text = result;
        //}

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.progressBar.Value = 0;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var dr = openFileDialog.ShowDialog();
            if (dr == false)
                return;
           
            var name = openFileDialog.FileName;
            var httpClient = new HttpClientUtil2(uploadurl);
            httpClient.SendProgress+= (s, arg) =>
            {
                Console.WriteLine("send: " + arg.ProgressPercentage);
                this.Dispatcher.Invoke(() => this.progressBar.Value = arg.ProgressPercentage);
            };

            httpClient.ReceiveProgress+= (s, arg) =>
            {
                Console.WriteLine("receive: " + arg.ProgressPercentage);
                //  this.Dispatcher.Invoke(() => this.progressBar.Value = arg.ProgressPercentage);
            };

            var result = await new HttpClientUtil2(uploadurl).Upload(formPath, new string[] { name }, new Dictionary<string, string>() { ["Authorization"] = token, ["dir"] = "newDir" });
            this.infoTextBox.Text = result;
        }


        private ProgressMessageHandler _progressHandler = new ProgressMessageHandler();
      
        private HttpClient GetClient(bool includeProgressHandler = false)
        {
            var handlers = new List<DelegatingHandler>();

            if (includeProgressHandler)
            {
                handlers.Add(_progressHandler);
            }

            var client = HttpClientFactory.Create(handlers.ToArray());
            client.BaseAddress = new Uri(uploadurl);
            return client;
        }
 

        private Task<HttpResponseMessage> UploadFile(HttpClient client,string FileName)
        {
            return client.PostAsJsonAsync("test", new
            {
                Foo = "Bar"
            });
        }



        private Task<HttpResponseMessage> PostUsingClient(HttpClient client)
        {
            return client.PostAsJsonAsync("test", new
            {
                Foo = "Bar"
            });
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var client = GetClient(true);
            var response = await PostUsingClient(client);
        }
    }
}
