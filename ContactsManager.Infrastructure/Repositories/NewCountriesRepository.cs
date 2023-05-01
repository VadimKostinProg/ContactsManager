using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseContext;
using Domain.Entities;
using Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class NewCountriesRepository : CountriesRepository, INewCountriesRepository
    {
        public NewCountriesRepository(ApplicationContext db) : base(db) { }

        public async Task<Country?> UpdateCountry(Country country)
        {
            Country? countryToUpdate = await _db.Countries
                .FirstOrDefaultAsync(c => c.CountryID == country.CountryID);

            if (countryToUpdate == null)
                return null;

            countryToUpdate.CountryName = country.CountryName;

            await _db.SaveChangesAsync();

            return countryToUpdate;
        }

        public async Task<bool> DeleteCountry(Guid id)
        {
            Country? country = await _db.Countries.FirstOrDefaultAsync(c => c.CountryID == id);

            if (country == null) 
                return false;

            _db.Countries.Remove(country);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
