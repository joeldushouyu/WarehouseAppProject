using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShellDemo.ViewModels; 
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ShellDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
            this.BindingContext = new SettingViewModel();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // check in viewmodel, to see if there is anymore 
            SettingViewModel vm = this.BindingContext as SettingViewModel;

            if (vm.NeedUpdate)
            {
                await DisplayAlert("Warning", "You still have orders in your account, please update all of them first", "OK");

                // push to orderGettedList
                await Shell.Current.GoToAsync($"//{nameof(OrderGettedListPage)}");
            }

        }
    }
}