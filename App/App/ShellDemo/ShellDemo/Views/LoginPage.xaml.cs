using ShellDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // check in viewmodel, to see if there is anymore 
            LoginViewModel vm = this.BindingContext as LoginViewModel;
            vm.ClearOutData();
            bool dangerInLogout = vm.DangerInLogout();
            if (dangerInLogout)
            {
                //warning user first
                await DisplayAlert("Warning", "You still have orders in your account, please update all of them first","OK");

                // push to orderGettedList
                await Shell.Current.GoToAsync($"//{nameof(OrderGettedListPage)}");


            }
            else
            {
                // do nothing,
                vm.Logout();
            }
            
            
            
        }
    }
}