using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Dtos.Responses
{
    public class StateResponseDto
    {
        [Required]
        public int StateId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
