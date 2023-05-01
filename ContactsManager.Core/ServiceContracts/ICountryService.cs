using DTO;

namespace ServiceContracts
{
    public interface ICountryService
    {
        /// <summary>
        /// Method for adding new country to data base.
        /// </summary>
        /// <param name="countryAddRequest">Country DTO object from request.</param>
        /// <returns>Country responce object, generated after adding 
        /// entity to data base with generated guid.</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Method for reading a collection of all country entities present in data base.
        /// </summary>
        /// <returns>Collection of objects with type ContryResponce</returns>
        Task<IEnumerable<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Method for read a country entity from data base with entere guid.
        /// </summary>
        /// <param name="countryID">Entity guid to search</param>
        /// <returns>Object of type CountryResponce
        /// (null if object with entered guid was not found)</returns>
        Task<CountryResponse?> GetCountry(Guid? countryID);
    }
}