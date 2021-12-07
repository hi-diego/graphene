using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Models
{
    public class ModelTracker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entries"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static IEnumerable<T> GenerateModelLogs<T>(IEnumerable<EntityEntry> entries, T log, object user = null) where T : IModelLog, new()
            => entries.Where(e => e.State != EntityState.Unchanged)
                .Select(entry => (new T()).InitializeGeneric<T>(entry, null))
                .ToList();
    }
}
