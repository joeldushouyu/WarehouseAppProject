using ShellDemo.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using ShellDemo.Models;

namespace ShellDemo.Views
{
    public partial class OrderGettedDetailPage : ContentPage
    {
        

        public OrderGettedDetailPage(OrderGettedDetailViewModel itemDetailViewModel)
        {
            InitializeComponent();
            BindingContext = itemDetailViewModel;
           
        }

      
    }
}