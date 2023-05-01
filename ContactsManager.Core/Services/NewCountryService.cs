using Domain.Entities;
using Domain.RepositoryContracts;
using DTO;
using Exceptions;
using Helpers;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NewCountryService : CountryService, INewCountryService
    {
        private readonly INewCountriesRepository _countriesRepository;

        public NewCountryService(INewCountriesRepository countriesRepository) : base(countriesRepository)
        {
            _countriesRepository= countriesRepository;
        }

        public async Task<CountryResponse> UpdateCountry(CountryUpdateRequest? countryUpdateRequest)
        {
            //Validation for null
            if(countryUpdateRequest == null)
                throw new ArgumentNullException(nameof(countryUpdateRequest));

            //Validation for porperties
            ValidationHelper.Validate(countryUpdateRequest);

            Country country = countryUpdateRequest.ToCountry();

            Country? countryUpdated = await _countriesRepository.UpdateCountry(country);

            //Validation: for not found
            if (countryUpdated == null)
                throw new ArgumentException("Country with passed guid is not found.");

            //Converting Person object to PersonResponce object and return
            return countryUpdated.ToCountryResponce();
        }

        public async Task<bool> DeleteCountry(Guid? guid)
        {
            //Validation: nullable guid
            if (guid == null)
                throw new InvalidPersonIDException(nameof(guid));

            //Matching country with passed ID
            bool result = await _countriesRepository.DeleteCountry(guid.Value);

            //Return result of deleting
            return result;
        }
    }
}
