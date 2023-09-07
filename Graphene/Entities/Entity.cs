using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Graphene.Extensions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Buffers;
using System.Text;
using System.Diagnostics;

namespace Graphene.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Entity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public override int Id { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class BaseEntity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [NotMapped]
        public virtual bool SerializeId { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Uuid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public virtual string? _Url { get => Uuid.ToBase64(); }

        /// <summary>
        /// 
        /// </summary>
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // public virtual Guid Uid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public virtual string _Entity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? DeletedAt { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseEntity()
        {
            _Entity = GetType().Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual Entity Update(object instance)
        {
            /*
            JsonObject json = (JsonObject)JsonObject.Parse(System.Text.Json.JsonSerializer.Serialize(instance, this.GetType()))!;
            JsonObject currentJson = (JsonObject)JsonObject.Parse(System.Text.Json.JsonSerializer.Serialize(this, this.GetType()))!;
            var outputBuffer = new ArrayBufferWriter<byte>();
            var jsonWriter = new Utf8JsonWriter(outputBuffer, new JsonWriterOptions { Indented = true });
            jsonWriter.WriteStartObject();
            foreach (var kv in json.AsEnumerable())
            {
                if (currentJson.ContainsKey(kv.Key)) currentJson.Remove(kv.Key);
                currentJson[kv.Key] = kv.Value.AsValue();
                currentJson.Add(kv.Key, kv.Value);
            }
            jsonWriter.WriteEndObject();
            return (BaseEntity) currentJson.Deserialize(this.GetType());
            */
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var newJsonString = System.Text.Json.JsonSerializer.Serialize(instance, instance.GetType(), serializeOptions);
            var originalJsonString = System.Text.Json.JsonSerializer.Serialize(this, this.GetType(), serializeOptions);
            var merged = SimpleObjectMerge(originalJsonString, newJsonString);
            return (Entity) System.Text.Json.JsonSerializer.Deserialize(merged, this.GetType(), serializeOptions);
        }



        public static string SimpleObjectMerge(string originalJson, string newContent)
        {
            var outputBuffer = new ArrayBufferWriter<byte>();

            using (JsonDocument jDoc1 = JsonDocument.Parse(originalJson))
            using (JsonDocument jDoc2 = JsonDocument.Parse(newContent))
            using (var jsonWriter = new Utf8JsonWriter(outputBuffer, new JsonWriterOptions { Indented = false,  }))
            {
                JsonElement root1 = jDoc1.RootElement;
                JsonElement root2 = jDoc2.RootElement;

                // Assuming both JSON strings are single JSON objects (i.e. {...})
                Debug.Assert(root1.ValueKind == JsonValueKind.Object);
                Debug.Assert(root2.ValueKind == JsonValueKind.Object);

                jsonWriter.WriteStartObject();

                // Write all the properties of the first document that don't conflict with the second
                foreach (System.Text.Json.JsonProperty property in root1.EnumerateObject())
                {
                    if (!root2.TryGetProperty(property.Name, out _))
                    {
                        property.WriteTo(jsonWriter);
                    }
                }

                // Write all the properties of the second document (including those that are duplicates which were skipped earlier)
                // The property values of the second document completely override the values of the first
                foreach (System.Text.Json.JsonProperty property in root2.EnumerateObject())
                {
                    property.WriteTo(jsonWriter);
                }

                jsonWriter.WriteEndObject();
            }

            return Encoding.UTF8.GetString(outputBuffer.WrittenSpan);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this, this.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Database"></param>
        public virtual void AfterAdded(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void AfterDeleted(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void AfterDetached(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void AfterModified(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void BeforeAdded(IGrapheneDatabaseContext Database)
        {
            CreatedAt = DateTime.Now;
        }

        public virtual void BeforeDeleted(IGrapheneDatabaseContext Database)
        {
            DeletedAt = DateTime.Now;
        }

        public virtual void BeforeDetached(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void BeforeModified(IGrapheneDatabaseContext Database)
        {
            ModifiedAt = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entry"></param>
        public static void SoftDelete(Entity entity, EntityEntry entry)
        {
            entity.DeletedAt = DateTime.Now;
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> QueryableOf<T>(T Entity)
        {
            return (new List<T>() { Entity }).AsQueryable();
        }
        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<dynamic> QueryableOf<T>() where T : class
        {
            var Object = Activator.CreateInstance(typeof(T), new object[] { });
            return (new List<T>() { ((T)Object) }).AsQueryable();
        }

        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<dynamic> DynamicQueryableOf<T>(T instance) where T : class
        {
            return (new List<T>() { instance }).AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldSerializeId()
        {
            return SerializeId;
        }
    }
}
