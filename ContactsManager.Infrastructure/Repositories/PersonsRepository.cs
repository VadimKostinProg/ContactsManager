using Domain.Entities;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataBaseContext;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationContext _db;

        public PersonsRepository(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();

            return person;
        }

        public async Task<bool> DeletePerson(Guid guid)
        {
            Person? person = await _db.Persons.FirstOrDefaultAsync(person => person.PersonID == guid);

            if (person == null)
                return false;

            _db.Persons.Remove(person);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Person>> GetAllPersons()
        {
            return _db.Persons.Include("Country").Select(person => person);
        }

        public async Task<IEnumerable<Person>> GetFilteredPersons(Func<Person, bool> predicate)
        {
            return _db.Persons.Include("Country").Where(predicate).ToList().Select(person => person);
        }

        public async Task<Person?> GetPersonByID(Guid guid)
        {
            return await _db.Persons.Include("Country").FirstOrDefaultAsync(person => person.PersonID == guid);
        }

        public async Task<Person?> UpdatePerson(Person person)
        {
            Person? personToUpdate = await _db.Persons.Include("Country")
                .FirstOrDefaultAsync(p => p.PersonID == person.PersonID);

            if (personToUpdate == null)
                return null;

            personToUpdate.Name = person.Name;
            personToUpdate.Email = person.Email;
            personToUpdate.DateOfBirth = person.DateOfBirth;
            personToUpdate.Gender = person.Gender?.ToString();
            personToUpdate.CountryID = person.CountryID;
            personToUpdate.Address = person.Address;
            personToUpdate.ReceiveNewsLetters = person.ReceiveNewsLetters;

            await _db.SaveChangesAsync();

            return personToUpdate;
        }
    }
}
