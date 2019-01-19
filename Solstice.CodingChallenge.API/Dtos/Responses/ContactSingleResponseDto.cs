using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Dtos.Responses
{
    public class ContactSingleResponseDto
    {
        [Required]
        public int ContactId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Company { get; set; }
        public string ProfileImageFileName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string PersonalPhoneNumber { get; set; }
        [Required]
        public int AddressId { get; set; }
        public AddressResponseDto Address { get; set; }
    }
}
