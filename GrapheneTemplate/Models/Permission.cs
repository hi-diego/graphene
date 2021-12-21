using Graphene.Entities;
using Graphene.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTemplate.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Permission : Entity, IAuthorizator
    {
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public int AuthorId { get => AuthorizedId; set => AuthorizedId = value; }
        /// <summary>
        /// 
        /// </summary>
        [Column("author_id")]
        public int AuthorizedId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Denied { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Entity { get; set; }
    }
}