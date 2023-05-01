using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Country entity.
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Method for adding new country to data store.
        /// </summary>
        /// <param name="country">Country object to add.</param>
        /// <returns>Country object which was inserted.</returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// Mothod that returns all countries from data store.
        /// </summary>
        /// <returns>Collection IEnumerable of Country objects.</returns>
        Task<IEnumerable<Country>> GetAllCountries();

        /// <summary>
        /// Method for reading the country from data store by it`s guid.
        /// </summary>
        /// <param name="guid">Guid of country to read.</param>
        /// <returns>Country object if country with passed guid 
        /// is present in data store, otherwise - null.</returns>
        Task<Country?> GetCountryByID(Guid guid);
    }
}
