using ContactsManager.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(50)]
        [DisplayName("Person name")]
        public string? PersonName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Email { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords are not same.")]
        public string? ConfirmPassword { get; set; }

        public UserOptions Options { get; set; } = UserOptions.Customer;
    }
}
