using DTO;
using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulation with person instance
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Method for adding new instance of person to data base.
        /// </summary>
        /// <param name="personAddRequest">Person object ot add to data base.</param>
        /// <returns>Person object which has been already added to data base with generated guid.</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Method that reads all persons from data base.
        /// </summary>
        /// <returns>Collection of PersonResponce type.</returns>
        Task<IEnumerable<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Method that reads person from data base with entered guid.
        /// </summary>
        /// <param name="personID">Guid of person to search.</param>
        /// <returns>Object of PersonResponce type(null if person with enetered guid is not found).</returns>
        Task<PersonResponse?> GetPerson(Guid? personID);

        /// <summary>
        /// Method that returns all persons from data base,
        /// which are mathed to the passed search field and search string.
        /// </summary>
        /// <param name="searchBy">Property name, on which filtering must be done.</param>
        /// <param name="searchString">Value of the property.</param>
        /// <returns>Returns collection of PersonResponce object.</returns>
        Task<IEnumerable<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString, string? gender = null);

        /// <summary>
        /// Method for sorting collection of persens by passed field in passed order.
        /// </summary>
        /// <param name="allPersons">Collection to sort.</param>
        /// <param name="sortBy">Fild to sort by.</param>
        /// <param name="sortOrder">Order to sort.</param>
        /// <returns>Collection of PersonResponce object.</returns>
        Task<IEnumerable<PersonResponse>> GetSortedPersons(IEnumerable<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Method for updating existing person in data base.
        /// </summary>
        /// <param name="personUpdateRequest">Object of PersonUpdateRequest with all fileds to update.</param>
        /// <returns>Object of PersonResponce.</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Method for deleting person from data base by personID.
        /// </summary>
        /// <param name="personID">Guid of person to delete.</param>
        /// <returns>If deleting was succesful it returns true, otherwise - false.</returns>
        Task<bool> DeletePerson(Guid? personID);
    }
}
