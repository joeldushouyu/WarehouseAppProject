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

        public void canSave()
        {
            int beginIndex = this.Locations.BinarySearch(InitialLocationByUser);
            int endIndex = this.Locations.BinarySearch(FinalLocationByUser);

            if (beginIndex > endIndex)
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
                Shell.Current.GoToAsync($"//{nameof(MainPage)}");

            }

        }
    }
}
