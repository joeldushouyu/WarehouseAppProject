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
        /*
        public List<Order> orders
        {
            get =>orders;
            set => orders = value;
        }*/

        public String BaseUrl => "http://10.0.2.2:5000";
        private MobileApp()
        {
            _user = new User();
            _locaDatabase = new Database();
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
