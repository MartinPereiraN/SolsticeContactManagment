using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solstice.CodingChallenge.Provider.Utilities
{
    static class QueryUtilities
    {
        public static IQueryable<T> DynamicInclude<T>(this IQueryable<T> query, string[] includes) where T : class
        {
            if (includes != null)
            {
                foreach (string include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}
