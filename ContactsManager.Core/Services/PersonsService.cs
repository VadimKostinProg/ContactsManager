using ServiceContracts;
using DTO; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Helpers;
using Enums;
using Domain.RepositoryContracts;
using Microsoft.Extensions.Logging;
using Exceptions;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ICountryService _countryService;

        public PersonsService(IPersonsRepository personsRepository, 
        ICountryService countryService)
        {
            _personsRepository = personsRepository;
            _countryService = countryService;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //Validation: personAddRequest
            if(personAddRequest == null) 
                throw new ArgumentNullException(nameof(personAddRequest));

            //Validating: personAddRequest.Name
            ValidationHelper.Validate(personAddRequest);

            //Creating Person object
            Person personToAdd = personAddRequest.ToPerson();

            //Adding Person object to data base
            Person person = await _personsRepository.AddPerson(personToAdd);

            //Converting Person object to PersonResponce object and return
            return person.ToPersonResponce();
        }

        public async Task<IEnumerable<PersonResponse>> GetAllPersons()
        {
            return (await _personsRepository.GetAllPersons()).Select(person => person.ToPersonResponce());
        }

        public async Task<PersonResponse?> GetPerson(Guid? personID)
        {
            //Validation: personID
            if (personID == null)
                return null;

            //Serching for person with entered id
            Person? person = await _personsRepository.GetPersonByID(personID.Value);

            if (person == null)
                return null;

            //Converting Person object to PersonResponce object and return
            return person.ToPersonResponce();
        }

        public async Task<IEnumerable<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString, string? gender = null)
        {
            //Getting all person from data base
            IEnumerable<PersonResponse> allPersons = await this.GetAllPersons();

            IEnumerable<PersonResponse> matchedPersones = allPersons;

            //If filter parametres are passed, getting filtered collection
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchString))
            {
                matchedPersones = MatchCollection(allPersons, searchBy, searchString);
            }

            //If gender parametr is passed, filter collection by gender
            if(gender == "Male" || gender == "Female")
            {
                matchedPersones = matchedPersones.Where(person => person.Gender == gender);
            }

            //Return filtered collection
            return matchedPersones;
        }

        private IEnumerable<PersonResponse> MatchCollection(IEnumerable<PersonResponse> allPersons, 
            string searchBy, string searchString)
        {
            IEnumerable<PersonResponse> matchedPersones;
            Type personType = typeof(PersonResponse);

            switch (searchBy)
            {
                case nameof(PersonResponse.Name):
                case nameof(PersonResponse.Email):
                case nameof(PersonResponse.Country):
                case nameof(PersonResponse.Address):
                    matchedPersones = allPersons.Where(person =>
                    (!string.IsNullOrEmpty(personType.GetProperty(searchBy)?.GetValue(person)?.ToString()) ?
                    personType.GetProperty(searchBy).GetValue(person).ToString()
                    .Contains(searchString, StringComparison.OrdinalIgnoreCase) : true));
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchedPersones = allPersons.Where(person => (person.DateOfBirth != null) ?
                    person.DateOfBirth.Value.ToShortDateString().Contains(searchString) : true);
                    break;
                default:
                    matchedPersones = allPersons;
                    break;
            }

            return matchedPersones;
        }

        public async Task<IEnumerable<PersonResponse>> GetSortedPersons(IEnumerable<PersonResponse> allPersons, 
            string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            Type personType = typeof(PersonResponse);

            if(personType.GetProperty(sortBy) == null)
                return allPersons;

            IEnumerable<PersonResponse> sortedPersones = allPersons;

            switch(sortOrder)
            {
                case SortOrderOptions.ASC:
                    sortedPersones = allPersons.OrderBy(person => personType.GetProperty(sortBy).GetValue(person));
                    break;
                case SortOrderOptions.DESC:
                    sortedPersones = allPersons.OrderByDescending(person => personType.GetProperty(sortBy).GetValue(person));
                    break;
            }

            return sortedPersones;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            //Validation: for not nullable
            if(personUpdateRequest == null)
                throw new ArgumentNullException(nameof(personUpdateRequest));

            //Validation: for all required fields
            ValidationHelper.Validate(personUpdateRequest);

            Person personToUpdate = personUpdateRequest.ToPerson();

            Person person = await _personsRepository.UpdatePerson(personToUpdate);

            //Validation: for not found
            if (person == null)
                throw new ArgumentException("Person with passed guid is not found.");

            //Converting Person object to PersonResponce object and return
            return person.ToPersonResponce();
        }

        public async Task<bool> DeletePerson(Guid? personID)
        {
            //Validation: nullable guid
            if (personID == null)
                throw new InvalidPersonIDException(nameof(personID));

            //Matching person with passed ID
            bool result = await _personsRepository.DeletePerson(personID.Value);

            //Return result of deleting
            return result;
        }
    }
}
