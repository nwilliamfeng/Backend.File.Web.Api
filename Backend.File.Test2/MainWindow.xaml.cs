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

namespace Backend.File.Test2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //https://stackoverflow.com/questions/14597232/asp-net-web-api-client-progressmessagehandler-post-task-stuck-in-winform-app
            //https://stackoverflow.com/questions/20661652/progress-bar-with-httpclient
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private ProgressMessageHandler _progressHandler = new ProgressMessageHandler();
        private const string SERVICE_URL = "http://www.google.com.au";

        private HttpClient GetClient(bool includeProgressHandler = false)
        {
            var handlers = new List<DelegatingHandler>();

            if (includeProgressHandler)
            {
                handlers.Add(_progressHandler);
            }

            var client = HttpClientFactory.Create(handlers.ToArray());
            client.BaseAddress = new Uri(SERVICE_URL);
            return client;
        }

        private void PostUsingClient(HttpClient client)
        {
            var postTask = client.PostAsJsonAsync("test", new
            {
                Foo = "Bar"
            });

            var postResult = postTask.Result;

            MessageBox.Show("OK");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var client = GetClient())
            {
                PostUsingClient(client);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var client = GetClient(true))
            {
                PostUsingClient(client);
            }
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
