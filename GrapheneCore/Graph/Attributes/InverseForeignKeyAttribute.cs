using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Graph.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class InverseForeignKeyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string InverseForeignKey { get; set; }

        public InverseForeignKeyAttribute(string inverseForeignKey)
        {
            InverseForeignKey = inverseForeignKey;
        }
    }
}
