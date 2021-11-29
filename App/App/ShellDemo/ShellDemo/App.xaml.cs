using ShellDemo.Services;
using ShellDemo.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellDemo
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            //Here try to update the server will what it have in the order, since it might got kill within any time.
            // ignore all exceptions? good practice?
        }

        protected override void OnResume()
        {
        }
    }
}
