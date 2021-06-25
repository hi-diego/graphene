using GrapheneCore.Database;
using GrapheneCore.Database.Entities;
using GrapheneCore.Extensions;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Graphene.Database.Entities
{

    /// <summary>
    /// 
    /// </summary>
    public class Order : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public double? Value { get => Product?.Price * Quantity; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public double? Cost { get => Product?.Cost * Quantity; }

        /// <summary>
        /// The posible status of an order.
        /// </summary>
        public enum STATUS { CANCELED = 0, CREATED, IN_PROCESS, READY, DELIVERED, CLEANED }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string ProductName { get => Product?.Name; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string SpaceName { get => Space?.Name; }

        /// <summary>
        /// 
        /// </summary>
        public double Quantity { get; set; } // Quantity without unit

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; } // Gramage

        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string For { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SpaceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(SpaceId))]
        public Space Space { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(CreatedById))]
        public User CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BillId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(BillId))]
        public Bill Bill { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Order()
        {
            For = Bill?.By;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Database"></param>
        /// <returns></returns>
        public override void AfterAdded(IDatabaseContext Database) // , IHubContext<MainHub> HubContext = null)
        {
            ConsumeStockAndNotify(Database);
            base.AfterAdded(Database); // , HubContext);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConsumeStockAndNotify(IDatabaseContext Database)
        {
            Database.Context context = (Database.Context)Database;
            context.Update(this);
            context.Entry(this).Reference(o => o.Product).Query().Includes(new string[] { "ChilldRelations.Chilld" }).FirstOrDefault();
            context.Entry(this).Reference(o => o.Space).Load();
            ConsumeStock();
            //context.HubContext.Clients.All.SendCoreAsync("Message", new[] { ToJson() }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Database"></param>
        /// <returns></returns>
        public override void BeforeModified(IDatabaseContext Database)//, IHubContext<MainHub> HubContext = null)
        {
            ConsumeStockAndNotify(Database);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ConsumeStock()
        {
            if (Status > 1)
            {
                Product.UpdateStock(this);
            }
        }
    }
}
