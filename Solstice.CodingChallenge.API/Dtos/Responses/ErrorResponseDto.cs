using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Dtos.Responses
{
    public class ErrorResponseDto
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
