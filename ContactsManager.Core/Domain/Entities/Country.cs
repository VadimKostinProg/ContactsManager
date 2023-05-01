using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Domain class for country
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }

        [Required]
        public string? CountryName { get; set; }

        public Country()
        {
            CountryID = Guid.NewGuid();
        }
    }
}