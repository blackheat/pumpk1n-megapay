using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace pumpk1n_backend.Mappings
{
    public static class AutoMapperConfigurator
    {
        public static IMapper LoadMapsFromAssemblies()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                var types = Assembly.GetExecutingAssembly().GetExportedTypes();
                LoadMappings(configuration, types);
            });

            return mapperConfiguration.CreateMapper();
        }

        private static void LoadMappings(IMapperConfigurationExpression cfg, Type[] types)
        {
            LoadIMapFromMappings(types);
            LoadIMapToMappings(types);
            LoadCustomMappings(cfg, types);
        }

        private static void LoadCustomMappings(IMapperConfigurationExpression cfg, IEnumerable<Type> types)
        {
            var maps = types
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.GetInterfaces()
                    .Any(i => typeof(IHaveCustomMappings).IsAssignableFrom(t)))
                .Select(t => (IHaveCustomMappings)Activator.CreateInstance(t))
                .ToArray();

            foreach (var map in maps)
            {
                map.CreateMappings(cfg);
            }
        }

        private static void LoadIMapFromMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select new
                        {
                            Source = i.GetGenericArguments()[0],
                            Destination = t
                        }).ToArray();

            foreach (var map in maps)
            {
                Mapper.Map(map.Source, map.Destination);
            }
        }

        private static void LoadIMapToMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select new
                        {
                            Destination = i.GetGenericArguments()[0],
                            Source = t
                        }).ToArray();

            foreach (var map in maps)
            {
                Mapper.Map(map.Source, map.Destination);
            }
        }
    }
}
