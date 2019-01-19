using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Dtos.Responses
{
    public class CityResponseDto
    {
        [Required]
        public int CityId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
