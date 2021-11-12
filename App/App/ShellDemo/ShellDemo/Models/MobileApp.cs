using System;
using System.Collections.Generic;
using System.Text;

namespace ShellDemo.Models
{
    public class MobileApp
    {

        // singleton class of the app
        private User _user = new User();
        public User User
        {
            get
            {
                return _user;
            }
        }

        public String BaseUrl => "http://127.0.0.1:5000";
        private MobileApp()
        {
            _user = new User();
        }

        private static MobileApp Singleton = new MobileApp();

        
        public static MobileApp GetSingletion()
        {
            return Singleton;
        }

    }
}
