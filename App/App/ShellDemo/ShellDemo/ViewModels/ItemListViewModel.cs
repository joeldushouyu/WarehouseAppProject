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

        public Boolean IsErrorMessageVisible => this._errorMessage.Length != 0;


        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(IsErrorMessageVisible));
            }
        }

        public ItemListViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(() => _ = ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

       
        }
        /// <summary>
        /// This function sends a request to the server to load List<Item> that is in the warehouse
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            
            try
            {
                // load data from server
                ErrorMessage = "";
                Items.Clear();


                List<Item>respond = await ServerRequest.LoadItemListRequest(MobileApp.GetSingletion().User.CurrentSessionUUID);

                foreach(Item item in respond)
                {
                    Items.Add(item);
                }
 


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
                    var s =IsErrorMessageVisible;  // call this, update all properities
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
 
            if (item == null)
                return;
            else
            {
                var page = new ItemDetailPage(new ItemDetailViewModel(item));
                _ = Shell.Current.Navigation.PushAsync(page);
            }


        }
    }
}