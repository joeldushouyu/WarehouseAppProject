using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ShellDemo.Models;
using ShellDemo.Views;

namespace ShellDemo.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        public List<string> Locations { get; set; }


        public bool NeedUpdate
        {
            get
            {
                return MobileApp.GetSingletion().User.Orders.Count != 0;
            }
        }
        public Command OnSave { get; }



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
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public string InitialLocationByUser { get; set; }
        public string FinalLocationByUser { get; set; }
        public SettingViewModel()
        {

            Locations = new List<string>();
            InitalizeLocationnList();
            OnSave = new Command(canSave);
        }

        public void InitalizeLocationnList()
        {
            List<string> Letters = new List<string>{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
                          "V", "W", "X", "Y", "Z"};
            foreach (string letter in Letters) {
                foreach (string let2 in Letters)
                {
                    Locations.Add(letter + let2);
                }
            }

            if (MobileApp.GetSingletion().User.CurrentSessionUUID == null)
            {
                // means the user has not login yet.
                ErrorMessage = "You have not login yet";
            }
            else
            {
                string workingSection = MobileApp.GetSingletion().User.WorkingSection;
                string[] zone = workingSection.Split('-');
                InitialLocationByUser = zone[0];
                FinalLocationByUser = zone[1];
            }
        }






        // direct copy code from UpdateOrderViewModel


        public void canSave()
        {
            ErrorMessage = "";

            int beginIndex = this.Locations.BinarySearch(InitialLocationByUser);
            int endIndex = this.Locations.BinarySearch(FinalLocationByUser);

            if(MobileApp.GetSingletion().User.Orders.Count != 0)
            {
                // means there are still orders, tell user that they have to update with server 
                ErrorMessage = "You have to update all your orders before continue";

            }
            else if (beginIndex > endIndex)
            {
                ErrorMessage = "Invalid choice";
            }else if (MobileApp.GetSingletion().User.CurrentSessionUUID == null)
            {
                ErrorMessage = "You have not logined yet!";
            }
            else
            {
                string newSection = InitialLocationByUser + "-" + FinalLocationByUser;
                MobileApp.GetSingletion().User.WorkingSection = newSection;
               
                // try to update the order, because the current orders picked by user might no be in this area
                
                Shell.Current.GoToAsync($"//{nameof(MainPage)}");

            }

        }
    }
}
