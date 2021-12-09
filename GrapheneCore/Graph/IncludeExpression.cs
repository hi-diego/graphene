using GrapheneCore.Extensions;
using GrapheneCore.Graph.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Graph
{
    /// <summary>
    /// 
    /// </summary>
    public class IncludeExpression
    {
        /// <summary>
        /// 
        /// </summary>
        public string IncludeString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsThenInclude { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsPrevMultiple { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public GraphType Relation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GraphType Root { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GraphType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IncludeExpression PreviousInclude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IncludeExpression(GraphType root, string raw, IGraph? graph = null, IncludeExpression? prevInclude = null)
        {
            Root = root;
            IsThenInclude = raw.StartsWith(".");
            IncludeString = IsThenInclude ? raw.Substring(1) : raw;
            PreviousInclude = prevInclude;
            string TypeName = IncludeString.Split("=>")[0].Trim().UcFirst();
            GraphType PrevField = PreviousInclude?.Type.Fields.Single(f => f.PascalName == TypeName);
            Type = PreviousInclude != null && IsThenInclude
                ? graph.Types.Single(t => t.SystemType == (PrevField.Multiple ? PrevField.SystemType.GetGenericArguments().First() : PrevField.SystemType))
                : root;
            IsPrevMultiple = PrevField != null ? PrevField.Multiple : false;
            if (graph != null)
            {
                // for the given include: Blog=>Blog.Posts.Take(10) the relation name is the second words if we split it by .
                //                   Blog=>Blog --->[Posts]<--- Take(10)
                string RelationName = IncludeString.Split(".")[1];
                Relation = Type.Fields.Single(f => f.PascalName == RelationName);
            }
        }
    }
}
