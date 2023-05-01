using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CountryUpdateRequest
    {
        [Required]
        [DisplayName("Country Id")]
        public Guid CountryID { get; set; }

        [Required]
        [DisplayName("Country Name")]
        public string? CountryName { get; set; }

        public Country ToCountry()
        {
            Country country = new Country()
            {
                CountryName = this.CountryName
            };

            country.CountryID = this.CountryID;

            return country;
        }
    }
}
