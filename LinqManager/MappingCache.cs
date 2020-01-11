using LinqManager.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LinqManager
{
    internal class MappingCache
    {
        private ConcurrentDictionary<string, List<Mapping>> _cache;

        public MappingCache()
        {
            _cache = new ConcurrentDictionary<string, List<Mapping>>();
        }

        public IReadOnlyList<Mapping> GetMapping<T>()
        {
            var mapping = _cache.GetOrAdd(typeof(T).Name, GetTypeMapping(typeof(T)));
            return mapping.AsReadOnly();
        }

        private List<Mapping> GetTypeMapping(Type type)
        {
            var mapping = type.GetProperties().Select(GetMapping).Where(m => m != null).ToList();

            return mapping;
        }

        private Mapping GetMapping(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<DtoMapAttribute>(false);
            if (attribute == null)
                return null;

            var props = attribute.DbPropertyName?.Split(':') ?? new string[0];

            return new Mapping { DbPropertyName = props.Count() > 0 ? props[0] : property.Name, FilterMethod = attribute.FilterMethod, 
                 DtoPropertyName = property.Name, DbCollectionPropertyName = props.Count() > 1 ? props[1] : string.Empty };
        }
    }
}