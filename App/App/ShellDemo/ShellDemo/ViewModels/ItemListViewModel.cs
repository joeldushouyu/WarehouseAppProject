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
using ShellDemo.Services;

namespace ShellDemo.ViewModels
{
    public class ItemListViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; }
       
        public Command<Item> ItemTapped { get; }

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

        public ItemListViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(() => _ = ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

       
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;


            // load data from singleton 
            
            try
            {
                // load data from server
                ErrorMessage = "";
                Items.Clear();

                string url = MobileApp.GetSingletion().BaseUrl + "/orderList";



                //JsonConvert.SerializeObject()
                List<Item>respond = await ServerRequest.LoadItemListRequest(MobileApp.GetSingletion().User.CurrentSessionUUID);

                foreach(Item item in respond)
                {
                    Items.Add(item);
                }
                //this.Items = new ObservableCollection<Item>(respond);
                //var items = await DataStore.GetItemsAsync(true);

            }
            catch (Exception ex)
            {
                /*
                if(ex.InnerException is FlurlHttpTimeoutException)
                {
                    ErrorMessage = "An internet error occurs, please try to refresth the page";
                }else if (ex.InnerException is OperationCanceledException)
                {
                    ErrorMessage = "An network error occurs, please try to refresh the page";
                } else if(ex.InnerException is FlurlHttpException)
                {
                    int errorcode = (int)((Flurl.Http.FlurlHttpException)ex.InnerException).StatusCode;
                    if(errorcode == 403)
                    {
                        ErrorMessage = "Invalid cridential, please try to relogin";
                    }
                    else
                    {
                        ErrorMessage = "Unknow network error occurs, please retry";
                    }
                }
                else
                {
                    ErrorMessage = "unknow error occurs, please retry";
                }*/
                ErrorMessage = ex.Message;
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

        public Item SelectedOrder
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }


        void OnItemSelected(Item item)
        {
            //TODO: modify
            if (item == null)
                return;
            else
            {
                var page = new ItemDetailPage(new ItemDetailViewModel(item));
                _ = Shell.Current.Navigation.PushAsync(page);
            }
           // var page = new ItemDetailPage(new ItemDetailViewModel(item));  // create a non nodal page
           // _ = Shell.Current.Navigation.PushAsync(page);

        }
    }
}