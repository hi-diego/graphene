# Graphene API-REST

Graphene Framework provides an auto generated API-REST, it acomplish this by creating a [Graph](https://graphene.software.freedom.icu/docs/graph) from your application models meta/data and exposing the BASICS operations: paginate (http:GET), read (http:GET), create (http:POST), update(http:PUT), delete(http:DELETE) through the ApiController abstract class.
IT also comes with a ready to use querying system and eagerLoading options

- Implementation (https://graphene.software.freedom.icu/docs/api-rest#implementation).
- Pagination (https://graphene.software.freedom.icu/docs/api-rest#pagination).
- Query (https://graphene.software.freedom.icu/docs/api-rest#query).
- Load Related Data (https://graphene.software.freedom.icu/docs/api-rest#load-related-data).

## Implementation

### Steps

1. Create a Controller and Inherit from ApiController.
2. Name your controller route, we recomend to use  "/graphene/api/" for the Route name.

### Example

```c#
using Microsoft.AspNetCore.Mvc;
using GrapheneCore.Database.Interfaces;

namespace Graphene.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("/graphene/api/")]
    public class ApiController : GrapheneCore.Http.Controllers.ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public ApiController(IGrapheneDatabaseContext dbContext, IConfiguration configuration) :base(dbContext, configuration)
        {
            //
        }
    }
}

```

## Read

You can verify the functionality through the auto generated API: [see](https://graphene.software.freedom.icu/docs/rest-api).

## Pagination

You can verify the functionality through the auto generated API: [see](https://graphene.software.freedom.icu/docs/rest-api).

## Querying

You can verify the functionality through the auto generated API: [see](https://graphene.software.freedom.icu/docs/rest-api).

## Load Related Data

The GraphType class is locaded in GrapheneCore.Graph.GraphType

## Create

The Graph singleton class is locaded in GrapheneCore.Graph.Graph

## Update

The GraphType class is locaded in GrapheneCore.Graph.GraphType

## Delete

The IGrapheneDatabaseContext interface is locaded in GrapheneCore.Database.IGrapheneDatabaseContext and The IGrapheneDatabaseContextExtensions are locaded in GrapheneCore.Database.Extensions.IGrapheneDatabaseContextExtensions

## Files

The ModelDictionary is class is locaded in GrapheneCore.Graph.GraphType
