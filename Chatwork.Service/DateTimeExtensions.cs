using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Chatwork.Service
{
    public static class DateTimeExtensions
    {
        internal static readonly  DateTime BaseDate = 
            new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static long ToUnixTime(this DateTime date)
        {
            var delta = date - BaseDate;
            if (delta.TotalSeconds < 0)
            {
                throw new ArgumentOutOfRangeException("date", "Unix epoc starts January 1st, 1970");
            }
            return (long)delta.TotalSeconds;
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            return BaseDate.AddSeconds(unixTime).ToLocalTime();
        }
    }

    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(
                    String.Format("Unexpected token parsing date. Expected Integer, got {0}.",
                    reader.TokenType));
            }

            var ticks = (long)reader.Value;
            return DateTimeExtensions.BaseDate.AddSeconds(ticks);
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                ticks = ((DateTime) value).ToUnixTime();
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
            writer.WriteValue(ticks);
        }
    }   
}
