using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Dtos.Requests
{
    public class BaseAddressDto
    {
        [Required]
        public int StateId { get; set; }
        public int? CityId { get; set; }
    }
}
