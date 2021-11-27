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
    public partial class UpdateOrderPage : ContentPage
    {
        public UpdateOrderPage(UpdaterOrderViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }
    }
}