using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace Controllers
{
    [Route("countries")]
    public class CountriesController : Controller
    {
        private readonly IFileInputService _fileInputService;

        public CountriesController(IFileInputService fileInputService)
        {
            _fileInputService = fileInputService;
        }

        [Route("upload-countries")]
        [HttpGet]
        public IActionResult UploadCountries()
        {
            return View();
        }

        [Route("upload-countries")]
        [HttpPost]
        public async Task<IActionResult> UploadCountries(IFormFile? excelFile)
        {
            if(excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please, choose correct file.";
                return View();
            }

            if(!Path.GetExtension(excelFile.FileName).Equals(".xlsx", 
                StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Upload valid extantion, \'xlsx\' was expected.";
                return View();
            }

            int countCountriesAdded = await _fileInputService.UploadCountriesFromExcel(excelFile);
            ViewBag.Message = $"{countCountriesAdded} countries inserted.";

            return View();
        }
    }
}
