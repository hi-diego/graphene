using Graphene.Entities;
using Graphene.Http.Converters;
using Graphene.Http.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTemplate.Database.Models
{
    public class Account : InstanceLog
    {
        /// <summary>
        /// 
        /// </summary>
        public enum PricingPlans  {
            Free,
            Basic,
            Premium
        }
        /// <summary>
        /// The name of the company associated to this account, this can be use 
        /// as the subdomain to separate the workspaces.
        /// </summary>
        /// 
        public string CompanyName { get; set; } = "Default";
        /// <summary>
        /// JSON cloud configuration 
        /// </summary>
        public string Configuration { get; set; } = "{}";
        /// <summary>
        /// Interal category that help our business to segment this Account Company
        /// is it a local family business? is it a Premium business?
        /// </summary>
        public string? CompanyCategory { get; set; }
        /// <summary>
        ///
        /// </summary>
        public PricingPlans Plan { get; set; }
    }
}
