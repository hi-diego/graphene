using GrapheneCore.Database.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Graphene.Database.Entities
{
    /// <summary>
    /// 
    /// </summary>
    //[NotMapped]
    public class DinnerTable : Space
    {

        /// <summary>
        /// 
        /// </summary>
        public enum TableStatus
        {
            FREE = 0,
            BUSSY,
            RESERVED,
            PAYING
        }

        /// <summary>
        /// 
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TableStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public IEnumerable<Reservation> Reservations { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    //[NotMapped]
    public class PayDesk : Space { }

    /// <summary>
    /// 
    /// </summary>
    public class Space : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public enum SpaceTypes
        {
            DinnerTable = 0,
            PayDesk,
            Bar
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Bill> Bills { get; set; } = new List<Bill>();

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public Bill ActiveBill { get => Bills.Where(b => !b.Payed).FirstOrDefault(); }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SpaceTypes Type { get; set; }
    }
}
