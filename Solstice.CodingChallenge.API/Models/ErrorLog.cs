using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Models
{
    public class ErrorLog
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
