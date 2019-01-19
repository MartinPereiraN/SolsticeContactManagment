using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Solstice.CodingChallenge.API.Dtos.Requests;
using Solstice.CodingChallenge.API.Dtos.Responses;
using Solstice.CodingChallenge.Domain.Models;
using Solstice.CodingChallenge.Provider;
using Solstice.CodingChallenge.Provider.Utilities;

namespace Solstice.CodingChallenge.API.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : BaseApiController
    {

        public ContactsController(IConfiguration configuration, IDatabaseProvider databaseProvider, IMapper mapper) : base(databaseProvider, mapper, configuration)
        {
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get(string email = null, string phoneNumber = null, int? pageNumber = 0, int? pageSize = null, string orderBy = null, string orderDirection = "ASC")
        {
            var contacts = _databaseProvider.GetFilteredContacts(email, phoneNumber, new string[] { "Address.City", "Address.State" });

            var pagedResult = GetPagedResults<Contact, ContactListResponseDto>(contacts, pageSize, pageNumber, orderBy ?? "ContactId", orderDirection); ;

            return Ok(pagedResult);
        }

        [HttpGet("byState/{stateId}")]
        public IActionResult GetByState([FromRoute]int stateId, int? cityId = 0)
        {
            var contacts = _databaseProvider.GetFromLocation(stateId, cityId, new string[] { "Address.City", "Address.State" });

            return Ok(_mapper.Map<List<ContactListResponseDto>>(contacts));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contact = await _databaseProvider.GetContact(id, new string[] { "Address.City", "Address.State" });

            if(contact == null)
            {
                return NotFound("There is no contact with the specified Id");
            }

            var response = _mapper.Map<ContactSingleResponseDto>(contact);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactCreateRequestDto contactRequest)
        {
            var fileValid = string.IsNullOrEmpty(contactRequest.ProfileImageFileName) || ValidFile(contactRequest.ProfileImageFileName); // It's not required so I only mark it invalid when there is a wrong value 
            var addressValid = ValidAddress(contactRequest.Address);
            if (!ModelState.IsValid || !fileValid || !addressValid)
            {
                return BadRequest(ModelState);
            }

            var contact = _mapper.Map<Contact>(contactRequest);

            _databaseProvider.CreateContact(contact);
            await _databaseProvider.Save();

            var result = _mapper.Map<ContactSingleResponseDto>(contact);

            return CreatedAtAction("GetContact", result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditContact([FromRoute] int id, [FromBody] ContactEditRequestDto contactRequest)
        {
            var fileValid = string.IsNullOrEmpty(contactRequest.ProfileImageFileName) || ValidFile(contactRequest.ProfileImageFileName);
            var addressValid = ValidAddress(contactRequest.Address);
            if (!ModelState.IsValid || !fileValid || !addressValid)
            {
                return BadRequest(ModelState);
            }

            var contact = _mapper.Map<Contact>(contactRequest);
            contact.ContactId = id;

            var editResult = _databaseProvider.EditContact(contact);
            await _databaseProvider.Save();


            if (editResult == (int)EditResults.ItemNotFound)
            {
                return NotFound("Register not found");
            }
            if(editResult == (int)EditResults.WrongEntityRelation)
            {
                ModelState.AddModelError("Entity Relation Error", "The address provided does not correspond with the contact provided");
                return BadRequest(ModelState);
            }

            return Ok(_mapper.Map<ContactSingleResponseDto>(contact));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deletedEntity = _databaseProvider.DeleteContact(id);
            await _databaseProvider.Save();

            if (deletedEntity == null)
            {
                return NotFound("Register not found");
            }

            return Ok(_mapper.Map<ContactSingleResponseDto>(deletedEntity));
        }
    }
}
