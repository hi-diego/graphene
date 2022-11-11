using Graphene.Entities;
using Graphene.Graph;
using Graphene.Http.Validation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using static Graphene.Graph.Graph;

namespace GrapheneTemplate.Database.Models
{
    public class IntToUidConverter : ValueConverter<int, Guid>
    {

        public IntToUidConverter(string name) : base(id => Graph.UIDS[name].GetGuid(id), guid => Graph.UIDS[name].GetId(guid))
        {
        }
    }

    public class JsonIntToUidConverter : JsonConverter
    {
        public JsonIntToUidConverter(string name)
        {
            Converter = new IntToUidConverter(name);
        }

        public IntToUidConverter Converter { get; }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return Converter.ConvertFromProvider((Guid.Parse((string) existingValue)));
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
           JToken t = JToken.FromObject(Converter.ConvertToProvider((int)value));
           t.WriteTo(writer);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Blog : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see AuthorUId
        /// </summary>
        [ValidForeignKey("Author")]
        [JsonProperty("AuthorUid")]
        [ForeignKey(nameof(Author))]
        [JsonConverter(typeof(JsonIntToUidConverter), "Author")]
        public virtual int AuthorId { get; set; }

        /// <summary>
        /// This computed property takes ands sets the correspondet UID and autoincrementable Ids
        /// to avoid Brute force attacks on AutoIncrementable Ids.
        /// </summary>
        //[NotMapped]
        //[JsonIgnore]
        //public virtual Guid AuthorUId
        //{
        //    // Get from Graph.UIDS GUID static cache dictionary.
        //    get => Graph.UIDS["Author"].GetGuid(AuthorId);
        //    // Set from Graph.UIDS ID static cache dictionary.
        //    set => AuthorId = Graph.UIDS["Author"].GetId(value);
        //}

        /// <summary>
        /// 
        /// </summary>
        public virtual Author? Author { get; set; }
    }
}
