using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Dtos.Responses
{
    public class AddressResponseDto
    {
        [Required]
        public int AddressId { get; set; }
        [Required]
        public string StreetInformation { get; set; }
        [Required]
        public int StateId { get; set; }
        public StateResponseDto State { get; set; }
        public int? CityId { get; set; }
        public CityResponseDto City { get; set; }
    }
}
