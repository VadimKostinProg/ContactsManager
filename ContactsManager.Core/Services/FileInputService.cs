using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using ServiceContracts;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FileInputService : IFileInputService
    {
        private readonly ICountryService _countryService;

        public FileInputService(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task<int> UploadCountriesFromExcel(IFormFile fileForm)
        {
            MemoryStream memoryStream = new MemoryStream();
            fileForm.CopyTo(memoryStream);

            int rowsAdded = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.First();

                int rows = excelWorksheet.Dimension.Rows;

                List<CountryResponse> countries = 
                    (await _countryService.GetAllCountries()).ToList();

                for(int row = 2; row <= rows; row++)
                {
                    string? countryName = Convert.ToString(excelWorksheet.Cells[row,1].Value);

                    if (string.IsNullOrEmpty(countryName))
                        continue;

                    if (countries.Any(country => country.CountryName == countryName))
                        continue;

                    CountryAddRequest countryAddRequest = new CountryAddRequest()
                    {
                        CountryName = countryName
                    };

                    await _countryService.AddCountry(countryAddRequest);
                    rowsAdded++;
                }
            }

            return rowsAdded;
        }
    }
}
