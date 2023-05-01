using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [Route("error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? feature = 
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            string errorMessage = "Error occured.";

            if(feature != null && feature.Error != null)
            {
                errorMessage = feature.Error.Message;
            }

            ViewBag.ErrorMessage = errorMessage;

            return View();
        }
    }
}
