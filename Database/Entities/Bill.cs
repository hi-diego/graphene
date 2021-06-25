using GrapheneCore.Database;
using GrapheneCore.Database.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Graphene.Database.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class CashDeposit : Bill { }

    /// <summary>
    /// 
    /// </summary>
    public class CashWithdrawal : Bill { }

    /// <summary>
    /// 
    /// </summary>
    public class BoxCut : Bill { }

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
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(SpaceId))]
        public Space Space { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SpaceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string By { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string JoinRequestBy { get; set; } = null;

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
        public IEnumerable<Order> Orders { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Database"></param>
        /// <returns></returns>
        public override void BeforeModified(IDatabaseContext Database) // , IHubContext<MainHub> HubContext = null)
        {
            Database.Context context = (Database.Context)Database;
            //if (JoinRequestBy != null) context.HubContext.Clients.All.SendCoreAsync("Message", new[] { ToJson() }).GetAwaiter().GetResult();
            base.AfterModified(Database);
        }
    }
}
