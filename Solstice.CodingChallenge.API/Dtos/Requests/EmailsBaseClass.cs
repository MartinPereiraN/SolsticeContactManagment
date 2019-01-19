using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Dtos.Requests
{
    public class EmailsBaseClass
    {
        public string WorkPhoneNumber { get; set; }
        public string PersonalPhoneNumber { get; set; }
    }

    internal class ContactPhoneValidator : AbstractValidator<EmailsBaseClass>
    {
        public ContactPhoneValidator()
        {
            RuleFor(contact => contact.WorkPhoneNumber).NotEmpty().When(contact => string.IsNullOrEmpty(contact.PersonalPhoneNumber));
            RuleFor(contact => contact.PersonalPhoneNumber).NotEmpty().When(contact => string.IsNullOrEmpty(contact.WorkPhoneNumber));
        }
    }
}
