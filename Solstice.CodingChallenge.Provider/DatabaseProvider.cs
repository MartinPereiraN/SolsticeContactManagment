using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Solstice.CodingChallenge.Domain.Data;
using Solstice.CodingChallenge.Domain.Models;
using Solstice.CodingChallenge.Provider.Repositories.Cities;
using Solstice.CodingChallenge.Provider.Repositories.Contacts;
using Solstice.CodingChallenge.Provider.Repositories.States;
using Solstice.CodingChallenge.Provider.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.Provider
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private ContactServiceProvider Contacts;
        private CityServiceProvider Cities;
        private StateServiceProvider States;

        ApplicationDbContext _context;

        public DatabaseProvider(ApplicationDbContext context)
        {
            Contacts = new ContactServiceProvider(context);
            Cities = new CityServiceProvider(context);
            States = new StateServiceProvider(context);

            _context = context;
        }

        // Contacts
        public IQueryable<Contact> GetFilteredContacts(string email = null, string phoneNumber = null, string[] includes = null)
        {
            return Contacts.GetFilteredContact(email, phoneNumber, includes);
        }

        public IQueryable<Contact> GetFromLocation(int stateId, int? cityId = 0, string[] includes = null)
        {
            return Contacts.Get(x => (x.Address.StateId == stateId) && (cityId == 0 || x.Address.CityId == cityId)).DynamicInclude(includes);
        }

        public async Task<Contact> GetContact(int id, string[] includes)
        {
            return await Contacts.GetContactById(id, includes);
        }

        public void CreateContact(Contact contact)
        {
            Contacts.Insert(contact);
            return;
        }

        public int EditContact(Contact contact)
        {
            return Contacts.EditContact(contact);
        }

        public Contact DeleteContact(int id)
        {
            return Contacts.Delete(id);
        }


        // Cities
        public IQueryable<City> GetCities()
        {
            return Cities.Get();
        }

        public bool CityExists(int id)
        {
            return Cities.Find(id) != null;
        }

        public bool CityExistsInState(int stateId, int cityId)
        {
            return Cities.Get(x => x.CityId == cityId && x.StateId == stateId).Any();
        }
        public void AddCity(City city)
        {
            Cities.Insert(city);
            return;
        }

        // States
        public IQueryable<State> GetStates()
        {
            return States.Get();
        }

        public bool StateExists(int id)
        {
            return States.Find(id) != null;
        }

        public IQueryable<City> GetStateCities(int stateId)
        {
            return Cities.Get(x => x.StateId == stateId);
        }
        public void AddState(State state)
        {
            States.Insert(state);
            return;
        }


        public void DetachEntity<T>(T item) where T: class
        {
            _context.Entry<T>(item).State = EntityState.Detached;
            return;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
            return;
        }
    }
}
