using Graphene.Graph.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace GrapheneTemplate.Database.Seeders
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISeeder
    {
        Task Seed(Database.GrapheneCache context, IGraph graph, IDistributedCache cache);
    }
}