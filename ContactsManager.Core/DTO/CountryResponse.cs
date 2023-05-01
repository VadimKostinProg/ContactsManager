using System;
using Domain.Entities;

namespace DTO
{
    /// <summary>
    /// DTO class that is used in the most methods of Country Service
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null) 
                return false;
            if(obj.GetType() != typeof(CountryResponse)) 
                return false;

            CountryResponse countryToCompare = (CountryResponse)obj;

            return this.CountryID == countryToCompare.CountryID && 
                this.CountryName == countryToCompare.CountryName;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExt
    {
        public static CountryResponse ToCountryResponce(this Country country)
        {
            return new CountryResponse() 
            { 
                CountryID = country.CountryID, 
                CountryName = country.CountryName 
            };
        }
    }
}
