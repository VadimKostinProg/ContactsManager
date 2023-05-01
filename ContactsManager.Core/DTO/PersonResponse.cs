using System;
using Domain.Entities;
using Enums;

namespace DTO
{
    /// <summary>
    /// DTO class for return Person object
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public int? Age 
        { 
            get
            {
                return GetAge();
            }
        }

        private int? GetAge()
        {
            if (DateOfBirth == null)
                return null;

            int age = DateTime.Now.Year - DateOfBirth.Value.Year;

            if(DateTime.Now.Month < DateOfBirth.Value.Month) age--;
            else if(DateTime.Now.Date < DateOfBirth.Value.Date) age--;

            return age;
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                Name = Name,
                Email = Email,
                DateOfBirth = DateOfBirth,
                CountryID = CountryID,
                Gender = !string.IsNullOrEmpty(Gender) ?
                (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, ignoreCase: true) : null,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if(obj.GetType() != typeof(PersonResponse)) 
                return false;

            PersonResponse personToCompare = (PersonResponse)obj;

            return this.PersonID == personToCompare.PersonID && this.Name == personToCompare.Name &&
                this.Email == personToCompare.Email && this.DateOfBirth == personToCompare.DateOfBirth &&
                this.Gender == personToCompare.Gender && this.CountryID == personToCompare.CountryID &&
                this.ReceiveNewsLetters == personToCompare.ReceiveNewsLetters;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class PersonExt
    {
        public static PersonResponse ToPersonResponce(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                Name = person.Name,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Country = person.Country?.CountryName
            };
        }
    }
}
