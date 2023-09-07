using Graphene.Entities;
using StackExchange.Redis;
using System.Text.RegularExpressions;

namespace Graphene.Cache
{
    public class RedisGuidCache {
        public IDatabase Redis { get; set; }
        public RedisGuidCache(IConnectionMultiplexer multiplexer)
        {
            Redis = multiplexer.GetDatabase();
        }

        public int GetCachedId (Guid guid)
        {
            string key = guid.ToString();
            string? stringId = Redis.StringGet(key).ToString().Split(":").Last();
            int id = stringId == null
                ? 0
                : Int32.Parse(stringId);
            return id;
        }

        public Tuple<int, string>? GetCached (Guid guid)
        {
            string key = guid.ToString();
            string? value = Redis.StringGet(key).ToString();
            if (value == null) return null;
            string? stringId = value.Split(":").Last();
            string? entityName = value.Split(":").First();
            int id = stringId == null
                ? 0
                : int.Parse(stringId);
            return new Tuple<int, string>(id, entityName);
        }

        public Guid GetCachedGuid (int id, string entityName)
        {
            string key = GetIdKey(id, entityName);
            string? guidString = Redis.StringGet(key).ToString();
            Guid guid = guidString == null
                ? Guid.Empty
                : new Guid(guidString);
            return guid;
        }

        public string GetIdKey (int id, string entityName)
        {
            return entityName + ":" +  id.ToString();
        }

        public string GetIdKey (Entity instance)
        {
            return GetIdKey(instance.Id, instance._Entity);
        }

        public void CacheIds (Entity instance)
        {
            string uid = (string)instance.Uuid.ToString();
            string id = (string)instance.Id.ToString();
            Redis.StringSetAsync(uid, GetIdKey(instance));
            Redis.StringSetAsync(GetIdKey(instance), uid);

            // Redis.SetAdd(uid, GetIdKey(instance));
            // Redis.SetAdd(GetIdKey(instance), uid);
        }

        public void CacheManyIds (IEnumerable<Entity> instances)
        {
            // instances
            // string uid = (string)instance.Uid.ToString();
            // string id = (string)instance.Id.ToString();
            // Redis.StringSetAsync(GetIdKey(instance), uid);
            // Redis.StringSetAsync(uid, GetIdKey(instance));
        }

        public Dictionary<string, string> GetGuidDictionary (IEnumerable<string> guids, bool full = true)
        {
            var uniqueGuids = guids.ToHashSet().ToArray();
            var idStrings = GetIds(uniqueGuids, full).ToArray();
            var i = 0;
            return idStrings.ToDictionary(v => uniqueGuids[i++]);
        }

        public IEnumerable<string> GetIds (IEnumerable<string> guids, bool full = true)
        {
            var redisKeys = guids.ToHashSet().Select(uid => new RedisKey(uid)).ToArray();
            var redisValues = Redis.StringGet(redisKeys);
            var idStrings = redisValues.Select(id => full ? $"{id}" : $"{id}".Split(':').Last());
            return idStrings;
        }

        public IEnumerable<string> GetIds (IEnumerable<Guid> guids)
        {
            return GetIds(guids.Select(g => g.ToString()));
        }
        public string ReplaceGuidsWithIds (string input)
        {
            string pattern = @"([\w]{8}-[\w]{4}-[\w]{4}-[\w]{4}-[\w]{12})";
            // match all the guids
            var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
            // filter and make them unique 
            var guids = matches.Select(m => m.Value);
            // Get dictionary: guids as keys, can posibly have null values, but not null keys
            var guidDic = GetGuidDictionary(guids, false);
            // replace it
            var output = guidDic.Aggregate(input, (o, gi) => o.Replace(gi.Key, gi.Value));
            return output;
        }

        public string ReplaceIdsWithGuids (string input)
        {
            string pattern = Pattern();
            // match all the guids
            var matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
            // filter and make them unique 
            var idsTemplate = matches.Select(m => m.Value.Split(Separator()).Last()).ToHashSet();
            // Get dictionary: guids as keys, can posibly have null values, but not null keys
            var ids = GetIds(idsTemplate).ToArray();
            // init index to acces ids array
            var i = 0;
            // replace it
            var output = idsTemplate.Aggregate(input, (o, replace) => o = o.Replace(ReplaceString(replace), ids[i++]));
            return output;
        }
        public static string FormatId (string entityName, object? id) => ReplaceString($"{entityName}:{id}");
        public static string Separator () => "REDIS_REPLACE";
        public static string ReplaceString (string value) => $"{Separator()}{value}";
        public static string SubPattern() => @"\w*:\d*";
        public static string Pattern() => ReplaceString($"({SubPattern()})");
    }
}
