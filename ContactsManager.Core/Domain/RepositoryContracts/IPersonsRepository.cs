using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Person entity.
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Method for inserting new person to the data store.
        /// </summary>
        /// <param name="person">Person object to insert.</param>
        /// <returns>Person object that was inserted.</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Method that returns all persons from data store.
        /// </summary>
        /// <returns>Collection IEnumerable of Person objects.</returns>
        Task<IEnumerable<Person>> GetAllPersons();

        /// <summary>
        /// Method to read person from data store by it`s guid.
        /// </summary>
        /// <param name="guid">Guid of person to read.</param>
        /// <returns>Person object with passed guid if it`s present 
        /// in data store, otherwise - null.</returns>
        Task<Person?> GetPersonByID(Guid guid);

        /// <summary>
        /// Method thar returns collection of persons from data store that satisfy condition.
        /// </summary>
        /// <param name="predicate">Delegate of type Func<Person, bool> which is 
        /// playing role of predicate for filtering the persons.</param>
        /// <returns>Collection IEnumerable of filtered Person objects.</returns>
        Task<IEnumerable<Person>> GetFilteredPersons(Func<Person, bool> predicate);

        /// <summary>
        /// Method for updating person.
        /// </summary>
        /// <param name="person">Person object to update.</param>
        /// <returns>Person object which was updated.</returns>
        Task<Person> UpdatePerson(Person person);

        /// <summary>
        /// Method for deleting person by it`s guid.
        /// </summary>
        /// <param name="guid">Guid of person to delete.</param>
        /// <returns>True, if deleting was successful, otherwise - false.</returns>
        Task<bool> DeletePerson(Guid guid);
    }
}
