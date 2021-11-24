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




        private string _pass;

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
           
        }

      
        
    }


    public class UserConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            User user = (User)value;

            writer.WriteValue(user.AccountName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            User user = new User();
            user.AccountName = (string)reader.Value;

            return user;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(User);
        }
    }
}
