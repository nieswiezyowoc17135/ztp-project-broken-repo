using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using ProjektZTP.Data;
using static ProjektZTP.Adapter.AdapterPattern;

namespace ProjektZTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IDataExporter _dataExporter;

        public FilesController(IDataExporter dataExporter)
        {
            _dataExporter = dataExporter;
        }

        [HttpGet("Excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var excelExporter = new ExcelExporter();
            var excelFile = await excelExporter.GenerateExcelFile(_dataExporter);
            var fileName = "Orders.xlsx";
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("Json")]
        public async Task<IActionResult> ExportJson()
        {
            var jsonExporter = new JsonExporter();
            var jsonFile = await jsonExporter.GenerateJsonFile(_dataExporter);
            var fileName = "Orders.json";
            return File(jsonFile, "application/json", fileName);
        }
    }
}
