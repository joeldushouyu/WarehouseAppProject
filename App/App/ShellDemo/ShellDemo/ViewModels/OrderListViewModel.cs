using ShellDemo.Models;
using ShellDemo.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using ShellDemo.Services;
using Flurl.Http;
using System.Collections.Generic;

namespace ShellDemo.ViewModels
{
    public class OrderListViewModel : BaseViewModel
    {
        private Order _selectedOrder;

        public ObservableCollection<Order> Orders { get; }
        public Command LoadOrdersCommand { get; }
       
        public Command<Order> OrderTapped { get; }

        public Boolean isErrorMessageVisible => this._errorMessage.Length != 0;


        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(isErrorMessageVisible));
            }
        }

        public OrderListViewModel()
        {
            Title = "Browse";
            Orders = new ObservableCollection<Order>();
            LoadOrdersCommand = new Command(() => _ = ExecuteLoadItemsCommand());

            OrderTapped = new Command<Order>(OnOrderSelected);

       
        }

        /// <summary>
        /// send request to Order to load Orders that is within User's picking range
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            ErrorMessage = "";

            try
            {
                // load data from server

                Orders.Clear();


     
                List<Order> message = await Services.ServerRequest.LoadOrderRequest(MobileApp.GetSingletion().User);



                foreach (var ord in message)
                {
                    this.Orders.Add(ord);
                }
                this._errorMessage = "";

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
                if (ErrorMessage.Length != 0)
                {
                    var s = isErrorMessageVisible;
                }
            }
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
                var page = new OrderDetailPage(new OrderDetailViewModel(item));
                _ = Shell.Current.Navigation.PushAsync(page);
            }


        }
    }
}