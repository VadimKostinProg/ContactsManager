using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace ServiceContracts
{
    public interface INewCountryService : ICountryService
    {
        /// <summary>
        /// Method for updating existing country in data base.
        /// </summary>
        /// <param name="countryUpdateRequest">Object of CountryUpdateRequest with all fields to update.</param>
        /// <returns>CountryResponse object.</returns>
        Task<CountryResponse> UpdateCountry(CountryUpdateRequest? countryUpdateRequest);

        /// <summary>
        /// Method for deleting country from data base.
        /// </summary>
        /// <param name="guid">Guid if country to delete.</param>
        /// <returns>If deleting was successful it returns true, otherwise - false.</returns>
        Task<bool> DeleteCountry(Guid? guid);
    }
}
