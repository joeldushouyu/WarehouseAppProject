using ShellDemo.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using ShellDemo.Models;

namespace ShellDemo.Views
{
    public partial class OrderDetailPage : ContentPage
    {
        

        public OrderDetailPage(OrderDetailViewModel itemDetailViewModel)
        {
            InitializeComponent();
            BindingContext = itemDetailViewModel;
           
        }

      
    }
}