﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Dtos.Requests
{
    public class ContactEditRequestDto: EmailsBaseClass
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Company { get; set; }
        public string ProfileImageFileName { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public AddressEditDto Address { get; set; }
    }
}
