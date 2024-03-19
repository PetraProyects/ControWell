using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControWell.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExcelController : ControllerBase
	{

		[HttpGet]
		[Route("Template")]
		public IActionResult ExportExcel3()
		{
			try
			{
				using (var workbook = new XLWorkbook(@"C:\Users\Dagoberto\Documents\ControWell\Client\wwwroot\FormatoCarrotanquesCargados.xlsx"))
				{
					var SampleSheet = workbook.Worksheets.Where(x => x.Name == "SLO-F-028").First();

					string CeldaItem = "C9";

					//*************************************************

					SampleSheet.Cell(CeldaItem).Value = 1200000;

					using var memoria = new MemoryStream();
					workbook.SaveAs(memoria);
					var nombreExcel = "Reporte.xlsx";
					var archivo = File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
					return archivo;
				}
			}
			catch (Exception)
			{
				throw;

			}
		}


			
	}
}
