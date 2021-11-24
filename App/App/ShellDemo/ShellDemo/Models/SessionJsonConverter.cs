using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShellDemo.Models
{
    public sealed class SessionJsonConverter: JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var tokenReader = reader as JTokenReader;
            var data = tokenReader.CurrentToken.ToString(Formatting.None);
            return data;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            UserSession sess = (UserSession)value;
            //writer.WriteToken(JsonToken.Raw, value);
            writer.WriteValue(sess.Session);
        }
    }
}
