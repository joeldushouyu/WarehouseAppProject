using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace ShellDemo.Models
{
    public class UserSession
    {
        private string _session;

        [JsonProperty("session")]
        public string Session
        {
            get {
                return _session;
            }
            set
            {
                _session = value;
            }
        }
    }
}
