using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Solstice.CodingChallenge.Domain.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        [Required]
        public string StreetInformation { get; set; }
        [Required]
        public int StateId { get; set; }
        public State State { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
    }
}
