using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace ShellDemo.Models
{
    public class User
    {
        private int _id;
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        
        private string _accountName = "";
        [JsonProperty("accountName")]
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                _accountName = value;
            }
        }

        private string _workingSection = "";
        public string WorkingSection
        {
            get
            {
                return _workingSection;
            }
            set
            {
                _workingSection = value;
            }
        }


        private UserSession _currentSession;
        public UserSession CurrentSession
        {
            get
            {
                return _currentSession;
            }
            set
            {
                _currentSession = value;
            }
        }


        private string _pass;
        [JsonProperty("password")]
        public string Password
        {
            set
            {
                _pass = value;
            }
            get => _pass;
        }


        public User()
        {
            _currentSession = null; // inidication that is not logined
        }

      
        
    }
}
