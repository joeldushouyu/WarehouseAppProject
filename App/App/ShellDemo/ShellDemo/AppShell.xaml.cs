using ShellDemo.ViewModels;
using ShellDemo.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ShellDemo.Models;

namespace ShellDemo
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            //initalize singletion
            Models.MobileApp.GetSingletion();  //initalize the singleton here
        } 

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

   
    }
}
