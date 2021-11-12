using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShellDemo.Models;

namespace ShellDemo.Views
{
    public partial class MainPage : ContentPage
    {
 
        public MainPage()
        {
            InitializeComponent();

            // if user is not login, turn the button to login, else
            MobileApp app = MobileApp.GetSingletion();
            if(app.User.CurrentSession == null)
            {
                // means not logined
                nextActionBtn.Text = "Login";
            }
            else
            {
                nextActionBtn.Text = "Browse OrderList page";
            }
        }

        private void BrowseItems_Clicked(object sender, EventArgs e)
        {   
            if(nextActionBtn.Text == "Login")
            {
                // go to login pgae
                Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                //Shell.Current.GoToAsync($"//{nameof(OrderListPage)}");
            }
            
        }
    }
}