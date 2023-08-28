using Graphene.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Graphene.Graph.Graph;
using Graphene.Http.Converters;
using Graphene.Http.Validation;

namespace GrapheneTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class User : Authenticable
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
        [InverseProperty(nameof(Models.Order.HandledBy))]
        public IEnumerable<Order> HandledOrders { get; set; } = new List<Order>();
        
        /// <summary>
        /// 
        /// /// 
        /// 
        /// </summary>
        [InverseProperty(nameof(Models.Order.CreatedBy))]
        public IEnumerable<Order> OrderHistory { get; set; } = new List<Order>();
        
    }
}
