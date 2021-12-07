using System;
using System.Collections.Generic;
using System.Text;
using ShellDemo.Services;
namespace ShellDemo.Models
{
    public class MobileApp
    {

        // singleton class of the app
        private User _user = new User { };
        public User User
        {
            get
            {
                return _user;
            }
        }



        private MobileApp()
        {
            _user = new User();
            _locaDatabase = new Database();
      
        }

        private Database _locaDatabase;  // local Database, incase when app is not able to synchronize with the server, so at least have a local record of what happened
        public Database LocalDatabase
        {
            get => _locaDatabase;
            set => _locaDatabase = value;
        }

        private static MobileApp Singleton = new MobileApp();  //lazy version of singleton

        
        public static MobileApp GetSingletion()
        {
            return Singleton;
        }

    }
}
