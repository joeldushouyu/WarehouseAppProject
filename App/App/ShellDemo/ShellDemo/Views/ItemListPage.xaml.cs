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
    public partial class ItemListPage : ContentPage
    {
        ItemListViewModel _viewModel;

        public ItemListPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}