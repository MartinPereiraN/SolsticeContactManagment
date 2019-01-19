using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Models
{
    public class PagedObject<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public decimal? TotalPages { get; set; }
    }
}
