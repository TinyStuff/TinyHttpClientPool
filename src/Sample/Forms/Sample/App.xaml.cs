using System;
using Sample.Services;
using TinyHttpClientPoolLib;
using Xamarin.Forms;

namespace Sample
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://jsonplaceholder.typicode.com";

        public App()
        {
            InitializeComponent();

            // Initialize like this, or simply create an instance of
            // TinyHttpClientPool and keep track of it yourself.
            TinyHttpClientPool.Initialize();

            // You can supply an action on what to do to initialize any new HttpClient
            // DO NOT keep a reference to the passed HttpClient since it must be controlled
            // by the pool.
            TinyHttpClientPool.Current.ClientInitialization = (obj) => obj.BaseAddress = new Uri(BackendUrl);

            DependencyService.Register<TodoService>();

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
