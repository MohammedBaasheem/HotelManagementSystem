using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class MapperHelper
    {
        public static IMapper Mapper { get; set; }

        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source)
        {
            return Mapper.Map<IEnumerable<TResult>>(source);
        }
        public static IQueryable<TResult> Map<TResult>(this IQueryable source)
            => source.ProjectTo<TResult>(Mapper.ConfigurationProvider);

        public static TResult MapOne<TResult>(this object source)
            => Mapper.Map<TResult>(source);

        public static void MapOne(this object source, object destination)
        {
            Mapper.Map(source, destination);
        }

        public static void MapTo<TSource, TDest>(this TSource source, TDest dest)
            => Mapper.Map(source, dest);
    }
}