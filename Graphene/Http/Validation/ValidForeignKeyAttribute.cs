using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ValidForeignKeyAttribute : ValidationAttribute
    {
        public ValidForeignKeyAttribute(string entity)
        {
            Entity = entity;
            this.ErrorMessage = "{0} Does not exist in: " + entity;
        }

        public string Entity { get; }

        public override bool IsValid(object? value)
        {
            int id = (value as int?) ?? 0;
            return Graphene.Graph.Graph.UIDS[Entity].Guid.ContainsKey(id);
        }
    }
}
