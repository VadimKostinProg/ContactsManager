using ServiceContracts;
using DTO;
using Domain.Entities;
using Domain.RepositoryContracts;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountriesRepository _countriesRepository;

        public CountryService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: null object
            if(countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            //Validation: null fields
            if(countryAddRequest.CountryName == null)
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            //Validation: duplicates
            if ((await _countriesRepository.GetAllCountries()).FirstOrDefault(country =>
            country.CountryName == countryAddRequest.CountryName) != null)
                throw new ArgumentException("Country with entered name is already exists in data base.");

            //Getting entity for data base from DTP class for adding
            Country countryToAdd = countryAddRequest.ToCountry();

            //Adding entity to data base
            Country country = await _countriesRepository.AddCountry(countryToAdd);

            //Return instance of DTP class for return
            return country.ToCountryResponce();
        }

        public async Task<IEnumerable<CountryResponse>> GetAllCountries()
        {
            List<Country> countries = (await _countriesRepository.GetAllCountries()).ToList();
            return countries.Select(country => country.ToCountryResponce());
        }
        public async Task<CountryResponse?> GetCountry(Guid? countryID)
        {
            //Validation: guid must be not null
            if (countryID == null)
                return null;

            //Searching country with entered guid
            Country? mathcedCountry = 
                await _countriesRepository.GetCountryByID(countryID.Value);

            //If country was not found return null
            if(mathcedCountry == null)
                return null;

            //Return the object of CountryResponce
            return mathcedCountry.ToCountryResponce();
        }
    }
}