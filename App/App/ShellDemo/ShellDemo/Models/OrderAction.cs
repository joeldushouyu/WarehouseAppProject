using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Newtonsoft.Json;

namespace ShellDemo.Models
{
    public class OrderAction
    {
        [JsonIgnore]
        [PrimaryKey, AutoIncrement]
        
        public int ID { get; set; }

        [JsonProperty("id")]
        public int IDAtDatabase { get; set; }

        [JsonProperty("fromOrderID")]
        public int FromOrderId { get; set; }

        [JsonProperty("action")]
        public string Action { get; set;}

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("itemBarcode")]
        public int ItemBarcode { get; set; }

        [JsonProperty("locationId")]
        public int LocationId { get; set; }

        [JsonProperty("status")]
        public bool Completed { get; set; }



        public string WorkingSection()
        {
            int counter = 0;
            string [] locationSymbol = new string []{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
              "V", "W", "X", "Y", "Z"};
            for (int i = 0; i < 26; i++)
            {
                for(int k = 0; k < 26; k++)
                {
                    for(int f = 0; f< 10; f++)
                    {
                        counter++;
                        if(counter == this.LocationId)
                        {
                            return locationSymbol[i] + locationSymbol[k] + f.ToString();
                        }
                    }
                }
            }
            return "Error";
        }
    }


}
