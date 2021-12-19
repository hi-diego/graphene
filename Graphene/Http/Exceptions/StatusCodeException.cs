using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Http.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class StatusCodeException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ActionResult Result;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public StatusCodeException(ActionResult result)
        {
            Result = result;
        }
    }
}
