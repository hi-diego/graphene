using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Extensions
{
    public static class JsonSerializerExtensions
    {
        public static IServiceProvider GetServiceProvider(this JsonSerializer serializer)
        {
            return serializer.Converters.OfType<IServiceProvider>().FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "No service provider found in JSON converters");
        }
    }
}
