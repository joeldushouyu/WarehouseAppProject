using ShellDemo.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using ShellDemo.Views;

namespace ShellDemo.ViewModels
{
    public class OrderGettedDetailViewModel : BaseViewModel
    {

        public string AssignedBarcode => ord.BarCode.ToString();
        public string Message => ord.Message;
        public string BoxSize => ord.BoxSize;
        public DateTime OrderDate => ord.OrderDate;

 
        public Command PickOrderCommand { get; }
        private Order ord;

        public OrderGettedDetailViewModel(Order item)
        {
            Title = "Order  Detail";
            this.ord = item;
            
        }


    }
}
