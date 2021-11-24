using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

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

        [JsonProperty("section")]
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

        

        private string _currentSessionUUID;
        [JsonProperty("session")]
        public string CurrentSessionUUID
        {
            get
            {
                return _currentSessionUUID;
            }
            set
            {
                _currentSessionUUID = value;
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

        /*
         internal void OnSerializingMethod(StreamingContext context)
         {
             Session = this._currentSession.Session;
         }*/
        private List<Order> _orders;
        public List<Order> Orders
        {
            get => _orders;
            set => value = _orders;
        }

        public int currentOrderIndex
        {
            // iterate each order, return the first one that currentOrderActionIndex is false
        }
        public User()
        {
            _currentSessionUUID = null; // inidication that is not logined
            // default working section
            _workingSection = "AA-AA";
            _orders = new List<Order>();
        }

      
        
    }
}
