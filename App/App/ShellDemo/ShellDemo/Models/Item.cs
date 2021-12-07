using System;
using Newtonsoft.Json;

namespace ShellDemo.Models
{
    public class Item
    {   

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("itemBarcode")]
        public string ItemBarcode { get; set; }

        [JsonProperty("image")]
        public byte[] Image { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("locationID")]
        public long LocationID { get; set; }

        [JsonProperty("notificationDate")]
        public DateTime NotificationDate { get; set; }

        [JsonProperty("notificationType")]
        public string NotificationType { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string SectionAndLocation { get; set; }

        
        /// <summary>
        /// The function receive an Item object, it will substitue all datas in the object with the data in the pass in object
        /// </summary>
        /// <param name="item"></param>
        public void UpdateInformation(Item item)
        {
            this.Id = item.Id;
            this.ItemBarcode = item.ItemBarcode;
            this.Image = item.Image;
            this.Quantity = item.Quantity;
            this.LocationID = item.LocationID;
            this.NotificationDate = item.NotificationDate;
            this.NotificationType = item.NotificationType;
            this.Name = item.Name;
            this.Weight = item.Weight;
        }

    }

}