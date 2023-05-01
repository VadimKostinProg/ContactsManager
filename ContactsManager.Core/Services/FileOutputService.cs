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
    public class FileOutputService : IFileOutputService
    {
        private readonly IPersonsService _personsService;

        public FileOutputService(IPersonsService personsService)
        {
            _personsService = personsService;
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");

                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Email";
                worksheet.Cells[1, 3].Value = "Date of birth";
                worksheet.Cells[1, 4].Value = "Gender";
                worksheet.Cells[1, 5].Value = "Country";
                worksheet.Cells[1, 6].Value = "Address";

                List<PersonResponse> persons = (await _personsService.GetAllPersons()).ToList();

                int row = 2;

                foreach(var person in persons)
                {
                    worksheet.Cells[row, 1].Value = person.Name;
                    worksheet.Cells[row, 2].Value = person.Email;
                    worksheet.Cells[row, 3].Value = person.DateOfBirth?.ToShortDateString();
                    worksheet.Cells[row, 4].Value = person.Gender;
                    worksheet.Cells[row, 5].Value = person.Country;
                    worksheet.Cells[row, 6].Value = person.Address;

                    row++;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
