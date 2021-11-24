using ShellDemo.Models;
using ShellDemo.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using Flurl.Http;
using System.Collections.Generic;

namespace ShellDemo.ViewModels
{
    public class OrderGettedListViewModel : BaseViewModel
    {
        private Order _selectedOrder;

        public ObservableCollection<Order> Orders { get; }
        public Command LoadOrdersCommand { get; }
       
        public Command<Order> OrderTapped { get; }
        public Command PickOrderCommand { get; }
       



        public OrderGettedListViewModel()
        {
            Title = "Browse";
            Orders = new ObservableCollection<Order>();
            LoadOrdersCommand = new Command(() =>  ExecuteLoadItemsCommand());

            OrderTapped = new Command<Order>(OnOrderSelected);
            PickOrderCommand = new Command(() => StartPicking());
       
        }
        private void StartPicking()
        {
            //TODO: add more logic

            var page = new PickRestockPage()
        }

        private void ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            //Orders = new ObservableCollection<Order>(MobileApp.GetSingletion().User.Orders);
            Orders.Clear();
            foreach (Order ord in MobileApp.GetSingletion().User.Orders)
            {
                Orders.Add(ord);
            }
            IsBusy = false;

        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedOrder= null;
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                SetProperty(ref _selectedOrder, value);
                OnOrderSelected(value);
            }
        }


        void OnOrderSelected(Order item)
        {
            //TODO: modify
            if (item == null)
                return;
            else
            {
                var page = new OrderGettedDetailPage(new OrderGettedDetailViewModel(item));
                _ = Shell.Current.Navigation.PushAsync(page);
            }
           // var page = new ItemDetailPage(new ItemDetailViewModel(item));  // create a non nodal page
           // _ = Shell.Current.Navigation.PushAsync(page);

        }
    }
}