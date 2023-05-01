using Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using DTO;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            PersonController controller = (PersonController)context.Controller;

            controller.ViewData["SearchFields"] = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.Name), "Person name"},
                {nameof(PersonResponse.Email), "Email"},
                {nameof(PersonResponse.DateOfBirth), "Date of Birth"},
                {nameof(PersonResponse.Country), "Country"}
            };

            controller.ViewData["Genders"] = new List<string>()
            {
                "Male",
                "Female",
                "Both"
            };

            IDictionary<string, object?>? properties = 
                (Dictionary<string, object?>?)context.HttpContext.Items["arguments"];

            if (properties == null)
            {
                return;
            }

            _logger.LogInformation("Index of PersonsController");

            if(properties.ContainsKey("searchBy"))
                controller.ViewData["CurrentSearchBy"] = properties["searchBy"];
            else
                controller.ViewData["CurrentSearchBy"] = String.Empty;

            if (properties.ContainsKey("searchString"))
                controller.ViewData["CurrentSearchString"] = properties["searchString"];
            else
                controller.ViewData["CurrentSearchString"] = String.Empty;

            if (properties.ContainsKey("gender"))
                controller.ViewData["CurrentGender"] = properties["gender"] ?? "Both";
            else
                controller.ViewData["CurrentGender"] = String.Empty;

            if (properties.ContainsKey("sortBy"))
                controller.ViewData["CurrentSortBy"] = properties["sortBy"];
            else
                controller.ViewData["CurrentSortBy"] = String.Empty;

            if (properties.ContainsKey("sortOrder"))
                controller.ViewData["CurrentSortOrder"] = properties["sortOrder"];
            else
                controller.ViewData["CurrentSortOrder"] = String.Empty;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;
            _logger.LogInformation("PersonListActionFilter executing.");
        }
    }
}
