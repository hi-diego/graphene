using Graphene.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Graphene.Graph.Graph;

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
        public override string Email { get => Identifier; set => Identifier = value; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Blog> Bolgs { get; set; } = new List<Blog>();
    }
}
