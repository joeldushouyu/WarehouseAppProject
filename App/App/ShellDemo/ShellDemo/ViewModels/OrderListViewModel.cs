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

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                // load data from server

                Orders.Clear();

                string url = MobileApp.GetSingletion().BaseUrl + "/orderList";



                //JsonConvert.SerializeObject()
                var act = await url.WithTimeout(30).PostJsonAsync(MobileApp.GetSingletion().User);
                List<Order>message = await act.GetJsonAsync<List<Order>>();
                //List<Item> message = List<Item>.from(parsedListJson.map((i) => Item.fromJson(i)));
           
                foreach (var ord in message)
                {
                    this.Orders.Add(ord);
                }
                this._errorMessage = "";
                //var items = await DataStore.GetItemsAsync(true);
                /*foreach (var item in items)
                {
                    Items.Add(item);
                }*/
            }
            catch(FlurlHttpTimeoutException e )
            {
                _errorMessage = " The network is unstable, please try to refresh the page";
            }
            catch(FlurlHttpException e)
            {
                int statusCode = (int)((Flurl.Http.FlurlHttpException)e).StatusCode;

                if(statusCode == 403)
                {
                    // means for some reason, the user was logged out
                    // let the user retry it

                    _errorMessage = "You are no longer logined, please try to login again!";
                }else if(statusCode == 500)
                {
                    _errorMessage = "an error occured on the server, please contact your manager";
                }
                else
                {
                    // 404 error code or potentially be other unsure error
                    _errorMessage = "Unknow error has occurred, please retry";
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                _errorMessage = "Unknow error has occurred, please retry";
            }
            finally
            {
                IsBusy = false;
                if (ErrorMessage.Length != 0)
                {
                    var s =isErrorMessageVisible;
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
           // var page = new ItemDetailPage(new ItemDetailViewModel(item));  // create a non nodal page
           // _ = Shell.Current.Navigation.PushAsync(page);

        }
    }
}