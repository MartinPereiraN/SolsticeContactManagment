using AutoMapper;
using Solstice.CodingChallenge.API.Dtos.Requests;
using Solstice.CodingChallenge.API.Dtos.Responses;
using Solstice.CodingChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.API.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Contact
            CreateMap<ContactCreateRequestDto, Contact>();
            CreateMap<ContactEditRequestDto, Contact>();
            CreateMap<Contact, ContactListResponseDto>().ForMember(dest => dest.AddressString, opt => opt.MapFrom(x => x.Address.StreetInformation + ", " + (x.Address.City == null ? "" : x.Address.City.Name) + ", " + x.Address.State.Name));
            CreateMap<Contact, ContactSingleResponseDto>();

            // Address
            CreateMap<AddressCreateDto, Address>();
            CreateMap<AddressEditDto, Address>();
            CreateMap<Address, AddressResponseDto>();

            // State
            CreateMap<State, StateResponseDto>();

            // City
            CreateMap<City, CityResponseDto>();
        }
    }
}
