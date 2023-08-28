using Graphene.Entities;
using Graphene.Graph;
using Graphene.Http.Converters;
using Graphene.Http.Validation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using static Graphene.Graph.Graph;

namespace GrapheneTemplate.Database.Models
{
    public class ProductPreference : Entity
    {
        public string Unit { get; set; } = "unit";
        public double Min { get; set; }
        public double Max { get; set; }
        public string DefaultComment { get; set; }
        [ValidForeignKey("User")]
        [ForeignKey(nameof(User))]
        [JsonConverter(typeof(GuidConverter<User>))]
        public virtual int? CreatedById { get; set; }
        public virtual User? CreatedBy { get; set; }
        [ValidForeignKey("Company")]
        [ForeignKey(nameof(Company))]
        [JsonConverter(typeof(GuidConverter<Company>))]
        public virtual int? BuyerId { get; set; }
        public virtual Company? Buyer { get; set; }
        [ValidForeignKey("Product")]
        [ForeignKey(nameof(Product))]
        [JsonConverter(typeof(GuidConverter<Product>))]
        public virtual int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
