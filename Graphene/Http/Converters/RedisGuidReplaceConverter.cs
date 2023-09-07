using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Graphene.Cache;
using System.Text.Json.Serialization;
using System.Globalization;

namespace Graphene.Http.Converters
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public class RedisGuidReplaceConverter<E> : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int id = 0;
            string readerValue = (string)reader.GetString();
            Int32.TryParse(readerValue, out id);
            return id;
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            => writer.WriteStringValue(RedisGuidCache.FormatId(typeof(E).Name, value));
    }
}
