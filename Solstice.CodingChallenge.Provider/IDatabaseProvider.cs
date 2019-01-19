using Microsoft.EntityFrameworkCore.ChangeTracking;
using Solstice.CodingChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solstice.CodingChallenge.Provider
{
    public interface IDatabaseProvider
    {
        // Contacts
        IQueryable<Contact> GetFilteredContacts(string email, string phoneNumber, string[] includes = null);
        IQueryable<Contact> GetFromLocation(int stateId, int? cityId = 0, string[] includes = null);
        Task<Contact> GetContact(int id, string[] includes);
        void CreateContact(Contact contact);
        int EditContact(Contact contact);
        Contact DeleteContact(int id);

        // Cities
        IQueryable<City> GetCities();
        bool CityExists(int id);
        bool CityExistsInState(int stateId, int cityId);
        void AddCity(City city);

        // States
        IQueryable<State> GetStates();
        bool StateExists(int id);
        IQueryable<City> GetStateCities(int stateId);
        void AddState(State state);

        void DetachEntity<T>(T item) where T : class;
        Task Save();
    }
}
