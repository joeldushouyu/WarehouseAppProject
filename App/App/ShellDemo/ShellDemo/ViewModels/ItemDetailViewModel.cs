using ShellDemo.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using ShellDemo.Views;
using ShellDemo.Models;
using ShellDemo.Services;
using Flurl.Http;

namespace ShellDemo.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {

        public string ItemBarCode => _itemChoose.ItemBarcode;
        public long Quantity => _itemChoose.Quantity;
        public string NotificationType => _itemChoose.NotificationType;
        public DateTime NotificationDate => _itemChoose.NotificationDate;
        public double Weight => _itemChoose.Weight;

        public string Name => _itemChoose.Name;

        public Command RefreshItemCommand { get; }

        private Item _itemChoose;
        private Item ItemChoose
        {
            get
            {
                return _itemChoose;
            }
            set
            {
                SetProperty(ref _itemChoose, value);
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Weight));
                OnPropertyChanged(nameof(NotificationDate));
                OnPropertyChanged(nameof(NotificationType));
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(ItemBarCode));
            }
        }
        private Item itemWithPart;

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                SetProperty(ref _errorMessage, value);
 
            }
        }

        public ItemDetailViewModel(Item item)
        {
            Title = "Item Detail";
            this._itemChoose = new Item();
            itemWithPart = item;
            LoadItem();

        }

        public async void LoadItem()
        {
            // first, get this item from the server
            try
            {
                IsBusy = true;
                var itemData = await Services.ServerRequest.LoadItemRequest(this.itemWithPart.LocationID, MobileApp.GetSingletion().User.CurrentSessionUUID);
  
                ItemChoose = itemData;

              

            }catch(Exception e)
            {
                ErrorMessage = e.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

 


    }
}
