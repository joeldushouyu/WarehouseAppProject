using ShellDemo.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using ShellDemo.Views;

namespace ShellDemo.ViewModels
{
    public class OrderDetailViewModel : BaseViewModel
    {

        public string AssignedBarcode => ord.barCode.ToString();
        public string Message => ord.Message;
        public string BoxSize => ord.BoxSize;
        public DateTime OrderDate => ord.OrderDate;

 
        public Command PickOrderCommand { get; }
        private Order ord;

        public OrderDetailViewModel(Order item)
        {
            Title = "Order  Detail";
            this.ord = item;
            PickOrderCommand = new Command(pickOrder);
        }

        void pickOrder()
        {
            // send to pick order page
            var page = new PickOrderPage(new PickOrderViewModel(this.ord));
            _ = Shell.Current.Navigation.PushAsync(page);
        }
    }
}
