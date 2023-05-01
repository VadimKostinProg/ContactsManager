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
    public class CountriesRepository : ICountriesRepository
    {
        protected readonly ApplicationContext _db;

        public CountriesRepository(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<Country> AddCountry(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return country;
        }

        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            return _db.Countries.Select(country => country);
        }

        public async Task<Country?> GetCountryByID(Guid guid)
        {
            return await _db.Countries.FirstOrDefaultAsync(country => country.CountryID == guid);
        }
    }
}
