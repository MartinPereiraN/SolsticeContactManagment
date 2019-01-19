using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Dtos.Requests
{
    public class AddressEditDto: BaseAddressDto
    {
        [Required]
        public int AddressId { get; set; }
        [Required]
        public string StreetInformation { get; set; }
    }
}
