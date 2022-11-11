using Graphene.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GrapheneDevelopmentTemplate.Database.Models
{
    public class Log : InstanceLog
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public override bool SerializeId { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Uid { get; set; } = Guid.NewGuid();
    }
}
