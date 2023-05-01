using Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountryService _countryService;

        public PersonCreateAndEditPostActionFilter(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is PersonController personController)
            {
                if(!personController.ModelState.IsValid)
                {
                    personController.ViewBag.Errors = personController.ModelState.Values.SelectMany(value => value.Errors)
                    .Select(error => error.ErrorMessage).ToList();

                    personController.ViewBag.Countries = (await _countryService.GetAllCountries()).ToList();

                    context.Result = personController.View();
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
