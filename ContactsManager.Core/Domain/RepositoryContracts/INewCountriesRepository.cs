using Domain.Entities;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    public interface INewCountriesRepository : ICountriesRepository
    {
        /// <summary>
        /// Method for updating existing country in data base.
        /// </summary>
        /// <param name="country">Country to update.</param>
        /// <returns>Update country.</returns>
        Task<Country> UpdateCountry(Country country);

        /// <summary>
        /// Method for deleting country from data base.
        /// </summary>
        /// <param name="country">Guid of country to delete.</param>
        /// <returns>If deleting was successful it returns true, otherwise - false.</returns>
        Task<bool> DeleteCountry(Guid id);
    }
}
