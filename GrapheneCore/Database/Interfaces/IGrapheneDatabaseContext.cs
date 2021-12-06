using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Database.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGrapheneDatabaseContext
    {
        /// <summary>
        /// This is the Declaration of what is going to be accesible by
        /// the API Interface, all the entities that are declared here are going
        /// to beaccesible through the ApiController.
        /// If the resource is not declared here the ApiController
        /// is going to return a 404 error.
        /// </summary>
        /// <typeparam name=""></typeparam>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public Dictionary<string, Type> ModelDictionary { get; set; }
    }
}
