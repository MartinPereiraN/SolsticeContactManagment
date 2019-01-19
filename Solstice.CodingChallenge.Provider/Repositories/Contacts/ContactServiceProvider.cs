using Solstice.CodingChallenge.Domain.Data;
using Solstice.CodingChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Solstice.CodingChallenge.Provider.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Solstice.CodingChallenge.Provider.Repositories.Contacts
{
    internal class ContactServiceProvider : Repository<Contact>
    {
        public ContactServiceProvider(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Contact> GetFilteredContact(string email = null, string phoneNumber = null, string[] includes = null)
        {
            return Get(x => (email == null || x.Email.Contains(email)) && (phoneNumber == null || (x.PersonalPhoneNumber.Contains(phoneNumber) || x.WorkPhoneNumber.Contains(phoneNumber)))).DynamicInclude(includes);
        }

        public Task<Contact> GetContactById(int id, string[] includes = null)
        {
            return (from item in _context.Contact
                    where item.ContactId == id
                    select item).DynamicInclude(includes).FirstOrDefaultAsync();
        }

        public int EditContact(Contact contact)
        {
            if (!ContactExists(contact.ContactId))
            {
                return (int)EditResults.ItemNotFound;
            }
            if (!CheckAddressAndContactRelated(contact))
            {
                return (int)EditResults.WrongEntityRelation;
            }
            _context.Entry(contact.Address).State = EntityState.Modified;
            _context.Entry(contact).State = EntityState.Modified;

            return (int)EditResults.Successful;
        }

        public Contact Delete(int id)
        {
            var contact = Get(x => x.ContactId == id).Include("Address").FirstOrDefault();
            if (contact == null)
            {
                return null;
            }

            _context.Remove(contact.Address);

            Delete(contact);

            return contact;
        }

        private bool ContactExists(int id)
        {
            return _context.Contact.Any(x => x.ContactId == id);
        }

        private bool CheckAddressAndContactRelated(Contact contact)
        {
            int? addressId = null;
            if (contact.AddressId != 0)
            {
                addressId = contact.AddressId;
            }
            else if (contact.Address.AddressId != 0)
            {
                addressId = contact.AddressId;
            }
            else
            {
                return false;
            }

            return _context.Contact.Any(x => x.ContactId == contact.ContactId && x.AddressId == contact.Address.AddressId);
        }
    }
}
