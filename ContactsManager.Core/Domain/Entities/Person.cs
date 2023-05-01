using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Domain class for person
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)]
        [Required]
        public string? Name { get; set; }

        [StringLength(40)]
        [Required]
        public string? Email { get; set; }

        [Required]
        [DisplayName("Date of birth")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [Required]
        [DisplayName("Country")]
        public Guid? CountryID { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }

        [ForeignKey("CountryID")]
        public Country? Country { get; set; }

        public Person()
        {
            PersonID = Guid.NewGuid();
        }
    }
}
