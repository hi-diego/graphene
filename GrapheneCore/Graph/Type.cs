using GrapheneCore.Extensions;
using GrapheneCore.Graph.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace GrapheneCore.Graph
{
    public class GraphType
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public Type? SystemType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // public virtual object Actions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string PascalName { get => Name.ToCamelCase(true); }

        /// <summary>
        /// 
        /// </summary>
        public virtual string HumanName { get => Name.ToCamelCase(true, true); }

        /// <summary>
        /// 
        /// </summary>
        public virtual string HumanNamePlural { get => Name.ToCamelCase(true, true).ToPlural(); }

        /// <summary>
        /// 
        /// </summary>
        public virtual string PluralName { get => Name.ToPlural(); }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // public virtual string[] Options { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // public virtual string Alias { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Multiple { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Primitive { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public string ForeignKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InverseProperty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string InverseForeignKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public IEnumerable<Rule> Rules { get; set; } = new List<Rule>();

        /// <summary>
        /// 
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual IEnumerable<GraphType> Fields { get; set; } = new List<GraphType>();

        /// <summary>
        /// 
        /// </summary>
        //public DatabaseContext DatabaseContext { get; }

        //public IEnumerable<Rule> DatabaseRules { get; set; } = new List<Rule>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public GraphType(Type value) // , IEnumerable<Rule> rules = null)
        {
            Init(value);
            //if (rules != null) DatabaseRules = rules;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public GraphType(PropertyInfo property)
        {
            Init(null, property);
        }

        /// <summary>
        /// 
        /// </summary>
        private GraphType Init(Type? systemType = null, PropertyInfo property = null)
        {
            SystemType = systemType ?? property.PropertyType;
            string name = (property?.Name ?? SystemType.Name);
            Name = name == "_Entity" ? name : name.ToCamelCase();
            Type = GetTypeName(SystemType).ToCamelCase();
            Multiple = property != null && typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string);
            Primitive = SystemType.IsEnum ? true : !SystemType.FullName.Contains("Entities");
            Fields = systemType == null
                ? null
                : SystemType.GetProperties()
                    .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() == null)
                    .Select(p => new GraphType(p)).ToList();
            ForeignKey = property?.GetCustomAttribute<ForeignKeyAttribute>()?.Name.ToCamelCase();
            InverseProperty = property?.GetCustomAttribute<InversePropertyAttribute>()?.Property;
            InverseForeignKey = property?.GetCustomAttribute<InverseForeignKeyAttribute>()?.InverseForeignKey;
            //Rules = property?.GetCustomAttributes<RuleAttribute>().Count() > 0
            //    ? new List<Rule>(property.GetCustomAttributes<RuleAttribute>().Select(r => r.ToRule(property.Name)))
            //    : new List<Rule>();
            //Rules = Rules.Concat(DatabaseRules);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(System.Type type)
        {
            if (type.IsEnum) return "list";
            if (type.GetGenericArguments().Count() > 0)
                return GetTypeName(type.GetGenericArguments()[0]);
            return type.Name.Replace("32", "").Replace("64", "");
        }
    }
}
