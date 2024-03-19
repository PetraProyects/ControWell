using Microsoft.AspNetCore.Mvc;
using System.Data;
using ClosedXML.Excel;
using System.Text.Json;

namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatoDescargueNafController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FormatoDescargueNafController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<FormatoDescargueNaf>>> GetFormatoDescargueNaf()
        {
            var lista = await _context.FormatoDescargueNafs.Include(x=>x.Ruta).ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<FormatoDescargueNaf>>> GetSingleFormatoDescargueNaf(int id)
        {
            var miobjeto = await _context.FormatoDescargueNafs.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<FormatoDescargueNaf>> CreateFormatoDescargueNaf(FormatoDescargueNaf objeto)
        {

            _context.FormatoDescargueNafs.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbFormatoDescargueNaf());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<FormatoDescargueNaf>>> UpdateFormatoDescargueNaf(FormatoDescargueNaf objeto)
        {

            var DbObjeto = await _context.FormatoDescargueNafs.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.NsvDescarga = objeto.NsvDescarga;


            await _context.SaveChangesAsync();

            return Ok(await _context.FormatoDescargueNafs.ToListAsync());


        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<FormatoDescargueNaf>>> DeleteFormatoDescargueNaf(int id)
        {
            var DbObjeto = await _context.FormatoDescargueNafs.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.FormatoDescargueNafs.Remove(DbObjeto);
            await _context.SaveChangesAsync();

            return Ok(await GetDbFormatoDescargueNaf());
        }


        private async Task<List<FormatoDescargueNaf>> GetDbFormatoDescargueNaf()
        {
            return await _context.FormatoDescargueNafs.ToListAsync();
        }

		[HttpGet]
		[Route("formatoslo029/{filtro}")]
		public IActionResult ExportExcel(string filtro)
		{

			try
			{
				using (var workbook = new XLWorkbook(@"C:\ControWell\Client\wwwroot\SLOF029FormatoDeCarrotanquesDescargados.xlsx"))
				{
					var consulta = JsonSerializer.Deserialize<Consulta>(filtro);
					var RegistrosAExportar = _context.FormatoDescargueNafs.Include(r => r.Ruta).Where(x => x.LlamadoDescargue >= consulta.FechaInicial && x.FinEntrega <= consulta.FechaFinal).ToList();
					var SampleSheet = workbook.Worksheets.Where(x => x.Name == "SLO-F-029").First();
					int NumeroFila = 5;
					int Item = 1;
					foreach (var i in RegistrosAExportar)
					{
						string Accion = "A" + Convert.ToString(NumeroFila);
						string DocDeTransporte = "B" + Convert.ToString(NumeroFila);
						string FinTransito = "C" + Convert.ToString(NumeroFila);
						string LamadoDescargar = "D" + Convert.ToString(NumeroFila);
						string InicioEntrega = "E" + Convert.ToString(NumeroFila);
						string FinEntrega = "F" + Convert.ToString(NumeroFila);
						string Origen = "G" + Convert.ToString(NumeroFila);
						string CodOrigen = "H" + Convert.ToString(NumeroFila);
						string Destino = "I" + Convert.ToString(NumeroFila);
						string CodDestino = "J" + Convert.ToString(NumeroFila);
						string CodRuta = "K" + Convert.ToString(NumeroFila);
						string Material = "L" + Convert.ToString(NumeroFila);
						string CodMaterial = "M" + Convert.ToString(NumeroFila);
						string Cedula = "N" + Convert.ToString(NumeroFila);
						string NombreC = "O" + Convert.ToString(NumeroFila);
						string Placa = "P" + Convert.ToString(NumeroFila);
						string Tanque = "Q" + Convert.ToString(NumeroFila);
						string Empresa = "R" + Convert.ToString(NumeroFila);
						string Sellos = "S" + Convert.ToString(NumeroFila);
						string Guia = "T" + Convert.ToString(NumeroFila);
						string GOV = "U" + Convert.ToString(NumeroFila);
						string GSV = "V" + Convert.ToString(NumeroFila);
						string Neto = "W" + Convert.ToString(NumeroFila);
						string BSW = "X" + Convert.ToString(NumeroFila);
						string TempF = "Y" + Convert.ToString(NumeroFila);
						string Api60F = "Z" + Convert.ToString(NumeroFila);
						string FacTemp = "AA" + Convert.ToString(NumeroFila);
						string IncertidumbreUPor = "AB" + Convert.ToString(NumeroFila);
						string IncertidumbreUBls = "AC" + Convert.ToString(NumeroFila);
						string Observ = "AD" + Convert.ToString(NumeroFila);
						string GUT2 = "AE" + Convert.ToString(NumeroFila);
						
						//*************************************************                     

						SampleSheet.Cell(Accion).Value = "CREAR";
						SampleSheet.Cell(DocDeTransporte).Value = Convert.ToString(i.DocTr);
						SampleSheet.Cell(FinTransito).Value = Convert.ToString(i.FinTransito.ToString("dd/MM/yyyy HH:mm"));
						SampleSheet.Cell(LamadoDescargar).Value = Convert.ToString(i.LlamadoDescargue.ToString("dd/MM/yyyy HH:mm"));
						SampleSheet.Cell(InicioEntrega).Value = Convert.ToString(i.InicioEntrega.ToString("dd/MM/yyyy HH:mm"));
						SampleSheet.Cell(FinEntrega).Value = Convert.ToString(i.FinEntrega.ToString("dd/MM/yyyy HH:mm"));
						SampleSheet.Cell(Origen).Value = Convert.ToString(i.Ruta.Origen);
						SampleSheet.Cell(CodOrigen).Value = Convert.ToString(i.Ruta.CodOrigen);
						SampleSheet.Cell(Destino).Value = Convert.ToString(i.Ruta.Destino);
						SampleSheet.Cell(CodDestino).Value = Convert.ToString(i.Ruta.CodDestino);
						SampleSheet.Cell(CodRuta).Value = Convert.ToString(i.Ruta.CodRuta);
						SampleSheet.Cell(Material).Value = Convert.ToString(i.MaterialDesc);
						SampleSheet.Cell(CodMaterial).Value = Convert.ToString(i.CodMaterial);
						SampleSheet.Cell(Cedula).Value = Convert.ToString(i.Cedula);
						SampleSheet.Cell(NombreC).Value = Convert.ToString(i.NombreConductor);
						SampleSheet.Cell(Placa).Value = Convert.ToString(i.Placa);
						SampleSheet.Cell(Tanque).Value = Convert.ToString(i.Tanque);
						SampleSheet.Cell(Empresa).Value = Convert.ToString(i.EmpresaTr);
						SampleSheet.Cell(Sellos).Value = Convert.ToString(i.Sellos);
						SampleSheet.Cell(Guia).Value = Convert.ToString(i.Guia);
						SampleSheet.Cell(GOV).Value = Convert.ToString(Math.Round(i.GovDescarga,2));
						SampleSheet.Cell(GSV).Value = Convert.ToString(Math.Round(i.GsvDescarga, 2));
						SampleSheet.Cell(Neto).Value = Convert.ToString(Math.Round(i.NsvDescarga, 2));
						SampleSheet.Cell(BSW).Value = Convert.ToString(Math.Round(i.BswDescarga, 2));
						SampleSheet.Cell(TempF).Value = Convert.ToString(Math.Round(i.TempDescarga, 2));
						SampleSheet.Cell(Api60F).Value = Convert.ToString(Math.Round(i.Api60Descarga, 2));
						SampleSheet.Cell(FacTemp).Value = Convert.ToString(Math.Round(i.FactorTempDescarga, 5));
						SampleSheet.Cell(IncertidumbreUPor).Value = Convert.ToString(Math.Round(i.IncertidumbreExpaPorcentDescarga, 2));
						SampleSheet.Cell(IncertidumbreUBls).Value = Convert.ToString(Math.Round(i.IncertidumbreExpaBslDescarga, 2));
						SampleSheet.Cell(Observ).Value = Convert.ToString(i.Observaciones);
						SampleSheet.Cell(GUT2).Value = Convert.ToString(i.Gut2);
						NumeroFila += 1;
						Item += 1;
					}
					using var memoria = new MemoryStream();
					workbook.SaveAs(memoria);
					var nombreExcel = "FormatoCarrotanquesCargados.xlsx";
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
