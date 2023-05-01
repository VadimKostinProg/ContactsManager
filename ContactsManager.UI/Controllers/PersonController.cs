using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using DTO;
using Enums;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using CRUDExample.Filters.ActionFilters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CRUDExample.Filters.ExceptionFilters;

namespace Controllers
{
    [Route("persons")]
    [TypeFilter(typeof(HandleExceptionFilter))]
    public class PersonController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly IPersonsService _personsService;
        private readonly IFileOutputService _fileOutputService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(ICountryService countryService, 
        IPersonsService personsService, IFileOutputService fileOutputService, 
        ILogger<PersonController> logger)
        {
            _countryService = countryService;
            _personsService = personsService;
            _fileOutputService = fileOutputService;
            _logger = logger;
        }

        [Route("index")]
        [Route("/")]
        [HttpGet]
        [TypeFilter(typeof(PersonsListActionFilter))]
        [TypeFilter(typeof(ResponseHeaderActionFilter), 
        Arguments = new object[] {"Custom-Key", "Custom-Value"})]
        public async Task<IActionResult> Index([FromQuery] string searchBy, [FromQuery] string? searchString,
        [FromQuery] string? gender, [FromQuery] string sortBy = nameof(PersonResponse.Name),
        SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            List<PersonResponse> persons =
                (await _personsService.GetFilteredPersons(searchBy, searchString, gender)).ToList();

            List<PersonResponse> sortedPersons = 
                (await _personsService.GetSortedPersons(persons, sortBy, sortOrder)).ToList();

            return View(sortedPersons);
        }

        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Create of PersonsController");

            ViewBag.Countries = (await _countryService.GetAllCountries())
                .Select(country => new SelectListItem() 
                { 
                    Text = country.CountryName, 
                    Value = country.CountryID.ToString() 
                });

            return View();
        }

        [Route("create")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        public async Task<IActionResult> Create(PersonAddRequest personRequest)
        {
            _logger.LogInformation("Create of PersonsController");

            PersonResponse personResponse = await _personsService.AddPerson(personRequest);

            return RedirectToAction("Index", "Person");
        }

        [Route("edit/{personsID}")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] Guid personsID)
        {
            _logger.LogInformation("Edit of PersonsController");

            PersonResponse? personResponce = await _personsService.GetPerson(personsID);

            if (personResponce == null)
                return RedirectToAction("Index");

            PersonUpdateRequest personUpdate = personResponce.ToPersonUpdateRequest();

            ViewBag.Countries = (await _countryService.GetAllCountries())
                .Select(country => new SelectListItem()
                {
                    Text = country.CountryName,
                    Value = country.CountryID.ToString()
                });

            return View(personUpdate);
        }

        [Route("edit/{personID}")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
        {
            _logger.LogInformation("Edit of PersonsController");

            PersonResponse? personResponse = await _personsService.GetPerson(personRequest.PersonID);

            if (personResponse == null)
                return NotFound("Person with entered ID not found.");

            await _personsService.UpdatePerson(personRequest);

            return RedirectToAction("Index");
        }

        [Route("delete/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid personID)
        {
            _logger.LogInformation("Delete of PersonsController");

            PersonResponse? personResponce = await _personsService.GetPerson(personID);

            if(personResponce == null)
            {
                return RedirectToAction("Index");
            }

            return View(personResponce);
        }

        [Route("delete/{personID}")]
        [HttpPost]
        public async Task<IActionResult> Delete(PersonResponse personResponse)
        {
            _logger.LogInformation("Delete of PersonsController");

            PersonResponse? personResponce = await _personsService.GetPerson(personResponse.PersonID);

            if (personResponce == null)
            {
                return NotFound("Person with entered ID not found.");
            }

            await _personsService.DeletePerson(personResponse.PersonID);

            return RedirectToAction("Index");
        }

        [Route("PersonsPDF")]
        [HttpGet]
        public async Task<IActionResult> PersonsPDF([FromQuery] string searchBy, [FromQuery] string? searchString,
        [FromQuery] string? gender, [FromQuery] string sortBy = nameof(PersonResponse.Name),
        SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("PersonsPDF of PersonsController");
            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, " +
                $"gender: {gender}, sortBy: {sortBy}, sortedOrder: {sortOrder}");

            List<PersonResponse> personsList =
                (await _personsService.GetFilteredPersons(searchBy, searchString, gender)).ToList();

            List<PersonResponse> sortedPersons =
                (await _personsService.GetSortedPersons(personsList, sortBy, sortOrder)).ToList();

            return new ViewAsPdf("PersonsPDF", sortedPersons, ViewData)
            {
                PageMargins = new Margins()
                {
                    Left = 20,
                    Top = 20,
                    Right = 20,
                    Bottom = 20
                },
                PageOrientation = Orientation.Landscape
            };
        }

        [Route("PersonsExcel")]
        [HttpGet]
        public async Task<IActionResult> PersonsExcel()
        {
            _logger.LogInformation("PersonsExcel of PersonsController");

            MemoryStream memoryStream = await _fileOutputService.GetPersonsExcel();

            return File(memoryStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "persons.xlsx");
        }
    }
}
