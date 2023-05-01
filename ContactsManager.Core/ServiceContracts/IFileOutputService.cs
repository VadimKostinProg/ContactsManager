using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IFileOutputService
    {
        /// <summary>
        /// Method that returns Excel file with table of persons.
        /// </summary>
        /// <returns>MemoryStream of Excel file.</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
