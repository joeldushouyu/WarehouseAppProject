using System;
using System.Collections.Generic;
using System.Text;
using ShellDemo.Services;
namespace ShellDemo.Models
{
    public class MobileApp
    {

        // singleton class of the app
        private User _user = new User { WorkingSection="AA"};
        //TODO: have to change the customize later
        public User User
        {
            get
            {
                return _user;
            }
        }

        public List<Item> ItemList;
        /*
        public List<Order> orders
        {
            get =>orders;
            set => orders = value;
        }*/


        private void InitailizeItemList()
        {
            this.ItemList = new List<Item>();
            List<string> alphabets = new List<string>{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
                                      "V", "W", "X", "Y", "Z" };
            int id = 1;
            foreach(string alph in alphabets)
            {
                foreach(string alph2 in alphabets)
                {
                    for(int i = 0; i < 10; i++)
                    {
                        Item tempItem = new Item();
                        tempItem.SectionAndLocation = alph + alph2 + i.ToString();
                        tempItem.LocationID = id;
                        id++;
                        this.ItemList.Add(tempItem);
                    }
                }
            }
                    
                


        }
        public String BaseUrl => "http://10.0.2.2:5000";
        private MobileApp()
        {
            _user = new User();
            _locaDatabase = new Database();
            InitailizeItemList();
        }

        private Database _locaDatabase;
        public Database LocalDatabase
        {
            get => _locaDatabase;
            set => _locaDatabase = value;
        }

        private static MobileApp Singleton = new MobileApp();

        
        public static MobileApp GetSingletion()
        {
            return Singleton;
        }

    }
}
