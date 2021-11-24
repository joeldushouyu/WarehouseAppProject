using ShellDemo.Models;
using ShellDemo.ViewModels;
using ShellDemo.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ShellDemo.Views
{
    public partial class OrderGettedListPage : ContentPage
    {
        OrderGettedListViewModel _viewModel;

        public OrderGettedListPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new OrderGettedListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}