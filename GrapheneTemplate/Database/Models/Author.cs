using Graphene.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GrapheneTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Author : Authenticable
    {
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string Email { get => Identifier; set => Identifier = value; }
        /// <summary>
        /// 
        /// </summary>
        [Column("email")]
        public override string Identifier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Permission> Permission { get; set; } = new List<Permission>();
    }
}
