using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Solstice.CodingChallenge.Domain.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Company { get; set; }
        public string ProfileImageFileName { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string PersonalPhoneNumber { get; set; }
        [Required]
        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
