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
            this.ErrorMessage = "Can't map the given Guid to Id, Please provide an {0} that exists. The provided {0} Does not exist in: " + entity;
        }

        public string Entity { get; }

        //public override string FormatErrorMessage(string name)
        //{
        //    return base.FormatErrorMessage(name).Replace("Id", "Uid");
        //}

        // public override bool IsValid(object? value)
        // {
        //     int id = (value as int?) ?? 0;
        //     //return Graphene.Graph.Graph.UIDS[Entity].Guid.ContainsKey(id);
        //     return id > 0 || value == null;
        // }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
           validationContext.MemberName = validationContext.MemberName.Replace("Id", "UId");
           validationContext.DisplayName = validationContext.DisplayName.Replace("Id", "UId");
           var r = new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
           var m = r.MemberNames;
           return ValidationResult.Success;
        }
    }
}
