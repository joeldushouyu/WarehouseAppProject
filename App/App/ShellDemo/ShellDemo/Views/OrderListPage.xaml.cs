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
using ShellDemo.ViewModels;

namespace ShellDemo.Views
{
    public partial class OrderListPage : ContentPage
    {
        OrderListViewModel _viewModel;

        public OrderListPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new OrderListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}