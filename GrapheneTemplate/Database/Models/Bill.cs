using Graphene.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Graphene.Entities;
using Graphene.Http.Converters;
using Graphene.Http.Validation;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace GrapheneTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    // public class CashDeposit : Bill { }

    // /// <summary>
    // /// 
    // /// </summary>
    // public class CashWithdrawal : Bill { }

    // /// <summary>
    // /// 
    // /// </summary>
    // public class BoxCut : Bill { }

    /// <summary>
    /// 
    /// </summary>
    public class Bill : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public enum BillType
        {
            Bill = 0,
            BoxCut,
            CashWithdrawal,
            CashDeposit
        }

        /// <summary>
        /// 
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ValidForeignKey("User")]
        [ForeignKey(nameof(User))]
        [JsonConverter(typeof(RedisGuidReplaceConverter<User>))]
        public int? UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Space? Space { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ValidForeignKey("Space")]
        [ForeignKey(nameof(Space))]
        [JsonConverter(typeof(RedisGuidReplaceConverter<Space>))]
        public int SpaceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // [JsonConverter(typeof(FetchCacheGuidConverter<Space>))]
        // [NotMapped]
        // public string SpaceUId { get => $"RedisReplace-{_Entity}-{SpaceId}"; }
        // public bool ShouldSerializeSpaceId_ () => false;

        /// <summary>
        /// 
        /// </summary>
        public BillType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Total { get; set; } = 0.0;

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public bool Payed { get => Total > 0; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public IEnumerable<object> OrdersSumary
        {
            get => Orders?.GroupBy(o => o.ProductId)
                ?.Select(g => new {
                    quantity = g.Count(),
                    productId = g.Key,
                    product = g.First()?.Product,
                    subTotal = g.First()?.Product?.Price ?? 0.0 * g.Count()
                });
        }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public double CalculatedTotal
        {
            get =>
                Total > 0.0
                ? Total
                : Orders?.Aggregate(0.0, (acc, o) => acc + o.Product?.Price ?? 0.0) ?? 0.0;
        }
    }
}