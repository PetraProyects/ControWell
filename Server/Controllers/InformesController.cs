using Microsoft.AspNetCore.Mvc;
using System.Data;
using ClosedXML.Excel;
using System.Text.Json;
namespace ControWell.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InformeController : ControllerBase
	{

		private readonly ApplicationDbContext _context;

		public InformeController(ApplicationDbContext context)
		{

			_context = context;
		}


		[HttpGet]
		public async Task<ActionResult<List<FormatoCarga>>> GetFormatoCarga()
		{
			var lista = await _context.FormatoCargas.Include(o => o.OfertaDiaria).Include(r => r.OfertaDiaria.Ruta).Include(c => c.OfertaDiaria.Conductor).Include(e => e.OfertaDiaria.Empresa).ToListAsync();
			return Ok(lista);
		}
		[HttpGet]
		[Route("{id}")]
		public async Task<ActionResult<List<FormatoCarga>>> GetSingleFormatoCarga(int id)
		{
			var miobjeto = await _context.FormatoCargas.FirstOrDefaultAsync(ob => ob.Id == id);
			if (miobjeto == null)
			{
				return NotFound(" :/");
			}

			return Ok(miobjeto);
		}
		[HttpPost]

		public async Task<ActionResult<FormatoCarga>> CreateFormatoCarga(FormatoCarga objeto)
		{

			_context.FormatoCargas.Add(objeto);
			await _context.SaveChangesAsync();
			return Ok(await GetDbFormatoCarga());
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<List<FormatoCarga>>> UpdateFormatoCarga(FormatoCarga objeto)
		{

			var DbObjeto = await _context.FormatoCargas.FindAsync(objeto.Id);
			if (DbObjeto == null)
				return BadRequest("no se encuentra");
			DbObjeto.FechaEnturne = objeto.FechaEnturne;
			DbObjeto.FechaLlamado = objeto.FechaLlamado;
			DbObjeto.FechaInicioLlenado = objeto.FechaInicioLlenado;
			DbObjeto.FechaFinLlenado = objeto.FechaFinLlenado;
			DbObjeto.FechaInicioTransito = objeto.FechaInicioTransito;


			await _context.SaveChangesAsync();

			return Ok(await _context.FormatoCargas.ToListAsync());


		}


		[HttpDelete]
		[Route("{id}")]
		public async Task<ActionResult<List<FormatoCarga>>> DeleteFormatoCarga(int id)
		{
			var DbObjeto = await _context.FormatoCargas.FirstOrDefaultAsync(Ob => Ob.Id == id);
			if (DbObjeto == null)
			{
				return NotFound("no existe :/");
			}

			_context.FormatoCargas.Remove(DbObjeto);
			await _context.SaveChangesAsync();

			return Ok(await GetDbFormatoCarga());
		}


		private async Task<List<FormatoCarga>> GetDbFormatoCarga()
		{
			return await _context.FormatoCargas.ToListAsync();
		}


		[HttpGet]
		[Route("ExportExcel")]
		public IActionResult ExportExcel()
		{
			List<ViewModelBalance> BalancesOrdenados = (from bal in _context.Balances
														select new ViewModelBalance
														{
															Fecha = bal.Fecha.ToString() ?? "",
															Tanque = bal.Tanque.NombreTanque,
															Pozo = "Nueva Esperanza",
															TipoMovimiento = bal.TipoMovimiento,

															Nivel = bal.NivelCorregido().ToString(),
															Tov = bal.Tov.ToString(),
															Interfase = bal.InterfaseCorregida().ToString(),

															Gov = bal.GOV().ToString(),
															Api = bal.Api.ToString(),
															TemFluido = bal.Tanque.TipoFluido,
															API60 = bal.Api60F().ToString(),
															TempeTanque = bal.TemTanqueCorregido().ToString(),
															Ctl = bal.CTL().ToString(),
															Gsv = bal.GSV().ToString(),
															Syw = bal.Syw.ToString(),
															Csw = bal.CSW().ToString(),
															Nsv = bal.Nsv.ToString(),
															AceiteProd = bal.DeltaNsv.ToString(),
															AceiteRecibi = "Delta",
															AceiteTransferido = "Delta",
															AceiteConsumido = "Delta",
															AceiteEntregado = "Delta",
															AguaNeta = bal.AGUANETA().ToString(),
															AguaProducida = bal.DeltaAguaNeta.ToString(),
															AguaRecibida = bal.DeltaAguaNeta.ToString(),
															AguaTransferida = "Delta",
															AguaConsumida = "Delta",
															AguaEntregada = "Delta",
															TemAmbiente = bal.TemAmbiente.ToString(),
															Fw = bal.Fw.ToString(),
															Ctsh = bal.Ctsh().ToString(),
															MedReal = bal.Nivel.ToString(),
															InterMed = bal.Interfase.ToString(),
															TemTanqMed = bal.TemTanque.ToString(),
															FactorCinta = bal.FactorCinta.ToString(),
															FactorInterface = bal.FactorInterface.ToString(),
														}
														  ).ToList();//Creo una lista ordenada por fecha


			try
			{
				DataTable table = new DataTable();//tabla general
				table.Columns.Add("FECHA");
				table.Columns.Add("TANQUE");
				table.Columns.Add("POZO");
				table.Columns.Add("INDICE");
				table.Columns.Add("MEDIDA TOTAL");
				table.Columns.Add("AFORO TOTAL");
				table.Columns.Add("MEDIDA AGUA");
				table.Columns.Add("AFORO AGUA");
				table.Columns.Add("VOL BRUTO OBSER");
				table.Columns.Add("API OBSER");
				table.Columns.Add("TEMP OBSER");
				table.Columns.Add("API 60°F");
				table.Columns.Add("TEMP TANQUE");
				table.Columns.Add("CORRECCION");
				table.Columns.Add("VOL BRUTO 60°F");
				table.Columns.Add("BSW %");
				table.Columns.Add("FACTOR BSW");
				table.Columns.Add("VOL NETO");
				table.Columns.Add("ACEITE PROD");
				table.Columns.Add("ACEITE RECIBI");
				table.Columns.Add("ACEITE TRANSFER");
				table.Columns.Add("ACEITE CONSUMO");
				table.Columns.Add("ACEITE ENTREGADO");
				table.Columns.Add("AGUA NETA");
				table.Columns.Add("AGUA PROD");
				table.Columns.Add("AGUA RECIBI");
				table.Columns.Add("AGUA TRANSFER");
				table.Columns.Add("AGUA CONSUMO");
				table.Columns.Add("AGUA ENTREGA");
				table.Columns.Add("TEM AMBIENTE");
				table.Columns.Add("CTSh");
				table.Columns.Add("MEDIDA REAL");
				table.Columns.Add("INTERFASE");
				table.Columns.Add("TEM REGISTRADA");
				table.Columns.Add("CORR CINTA");
				table.Columns.Add("FACTOR INTER");

				var BalancesOrdenadosPorFecha = BalancesOrdenados.OrderBy(b => b.Fecha).ToList();
				foreach (var item in BalancesOrdenadosPorFecha)
				{
					DataRow fila = table.NewRow();
					fila["FECHA"] = item.Fecha;
					fila["TANQUE"] = item.Tanque;
					fila["POZO"] = item.Pozo;
					fila["INDICE"] = item.TipoMovimiento;
					fila["MEDIDA TOTAL"] = item.Nivel;
					fila["AFORO TOTAL"] = item.Tov;
					fila["MEDIDA AGUA"] = item.Interfase;
					fila["AFORO AGUA"] = item.Fw;
					fila["VOL BRUTO OBSER"] = item.Gov;
					fila["API OBSER"] = item.Api;
					fila["TEMP OBSER"] = item.TemFluido;
					fila["API 60°F"] = item.API60;
					fila["TEMP TANQUE"] = item.TempeTanque;
					fila["CORRECCION"] = item.Ctl;
					fila["VOL BRUTO 60°F"] = item.Gsv;
					fila["BSW %"] = item.Syw;
					fila["FACTOR BSW"] = item.Csw;
					fila["VOL NETO"] = item.Nsv;
					fila["ACEITE PROD"] = item.AceiteProd;
					fila["ACEITE RECIBI"] = item.AceiteRecibi;
					fila["ACEITE TRANSFER"] = item.AceiteTransferido;
					fila["ACEITE CONSUMO"] = item.AceiteConsumido;
					fila["ACEITE ENTREGADO"] = item.AceiteEntregado;
					fila["AGUA NETA"] = item.AguaNeta;
					fila["AGUA PROD"] = item.AguaProducida;
					fila["AGUA RECIBI"] = item.AguaRecibida;
					fila["AGUA TRANSFER"] = item.AguaTransferida;
					fila["AGUA CONSUMO"] = item.AguaConsumida;
					fila["AGUA ENTREGA"] = item.AguaEntregada;
					fila["TEM AMBIENTE"] = item.TemAmbiente;
					fila["CTSh"] = item.Ctsh;
					fila["MEDIDA REAL"] = item.MedReal;
					fila["INTERFASE"] = item.InterMed;
					fila["TEM REGISTRADA"] = item.TemTanqMed;
					fila["CORR CINTA"] = item.FactorCinta;
					fila["FACTOR INTER"] = item.FactorInterface;
					table.Rows.Add(fila);
				};

				using var libro = new XLWorkbook();
				table.TableName = "Registros";

				var hoja = libro.Worksheets.Add(table);

				hoja.ColumnsUsed().AdjustToContents();
				//agregar tablas de tanques al excel
				foreach (var i in _context.Tanques)
				{
					DataTable tableNueva = new DataTable();
					//aqui los datos de ese tanque
					tableNueva.Columns.Add("FECHA");
					tableNueva.Columns.Add("TANQUE");
					tableNueva.Columns.Add("POZO");
					tableNueva.Columns.Add("INDICE");
					tableNueva.Columns.Add("MEDIDA TOTAL");
					tableNueva.Columns.Add("AFORO TOTAL");
					tableNueva.Columns.Add("MEDIDA AGUA");
					tableNueva.Columns.Add("AFORO AGUA");
					tableNueva.Columns.Add("VOL BRUTO OBSER");
					tableNueva.Columns.Add("API OBSER");
					tableNueva.Columns.Add("TEMP OBSER");
					tableNueva.Columns.Add("API 60°F");
					tableNueva.Columns.Add("TEMP TANQUE");
					tableNueva.Columns.Add("CORRECCION");
					tableNueva.Columns.Add("VOL BRUTO 60°F");
					tableNueva.Columns.Add("BSW %");
					tableNueva.Columns.Add("FACTOR BSW");
					tableNueva.Columns.Add("VOL NETO");
					tableNueva.Columns.Add("ACEITE PROD");
					tableNueva.Columns.Add("ACEITE RECIBI");
					tableNueva.Columns.Add("ACEITE TRANSFER");
					tableNueva.Columns.Add("ACEITE CONSUMO");
					tableNueva.Columns.Add("ACEITE ENTREGADO");
					tableNueva.Columns.Add("AGUA NETA");
					tableNueva.Columns.Add("AGUA PROD");
					tableNueva.Columns.Add("AGUA RECIBI");
					tableNueva.Columns.Add("AGUA TRANSFER");
					tableNueva.Columns.Add("AGUA CONSUMO");
					tableNueva.Columns.Add("AGUA ENTREGA");
					tableNueva.Columns.Add("TEM AMBIENTE");
					tableNueva.Columns.Add("CTSh");
					tableNueva.Columns.Add("MEDIDA REAL");
					tableNueva.Columns.Add("INTERFASE");
					tableNueva.Columns.Add("TEM REGISTRADA");
					tableNueva.Columns.Add("CORR CINTA");
					tableNueva.Columns.Add("FACTOR INTER");
					var LosBalancesDeEseTanque = BalancesOrdenados.Where(x => x.Tanque == i.NombreTanque).ToList();//en este caso como lo definí Tanque es el nombre del tanque
					var LosBalancesDeEseTanqueOrdenadoFecha = LosBalancesDeEseTanque.OrderBy(x => x.Fecha).ToList();
					foreach (var item in LosBalancesDeEseTanqueOrdenadoFecha)
					{
						DataRow fila = tableNueva.NewRow();
						fila["FECHA"] = item.Fecha;
						fila["TANQUE"] = item.Tanque;
						fila["POZO"] = item.Pozo;
						fila["INDICE"] = item.TipoMovimiento;
						fila["MEDIDA TOTAL"] = item.Nivel;
						fila["AFORO TOTAL"] = item.Tov;
						fila["MEDIDA AGUA"] = item.Interfase;
						fila["AFORO AGUA"] = item.Fw;
						fila["VOL BRUTO OBSER"] = item.Gov;
						fila["API OBSER"] = item.Api;
						fila["TEMP OBSER"] = item.TemFluido;
						fila["API 60°F"] = item.API60;
						fila["TEMP TANQUE"] = item.TempeTanque;
						fila["CORRECCION"] = item.Ctl;
						fila["VOL BRUTO 60°F"] = item.Gsv;
						fila["BSW %"] = item.Syw;
						fila["FACTOR BSW"] = item.Csw;
						fila["VOL NETO"] = item.Nsv;
						fila["ACEITE PROD"] = item.AceiteProd;
						fila["ACEITE RECIBI"] = item.AceiteRecibi;
						fila["ACEITE TRANSFER"] = item.AceiteTransferido;
						fila["ACEITE CONSUMO"] = item.AceiteConsumido;
						fila["ACEITE ENTREGADO"] = item.AceiteEntregado;
						fila["AGUA NETA"] = item.AguaNeta;
						fila["AGUA PROD"] = item.AguaProducida;
						fila["AGUA RECIBI"] = item.AguaRecibida;
						fila["AGUA TRANSFER"] = item.AguaTransferida;
						fila["AGUA CONSUMO"] = item.AguaConsumida;
						fila["AGUA ENTREGA"] = item.AguaEntregada;
						fila["TEM AMBIENTE"] = item.TemAmbiente;
						fila["CTSh"] = item.Ctsh;
						fila["MEDIDA REAL"] = item.MedReal;
						fila["INTERFASE"] = item.InterMed;
						fila["TEM REGISTRADA"] = item.TemTanqMed;
						fila["CORR CINTA"] = item.FactorCinta;
						fila["FACTOR INTER"] = item.FactorInterface;

						tableNueva.Rows.Add(fila);
					};
					//aqui finalizan los datos de ese tanque en especifico
					tableNueva.TableName = i.NombreTanque.ToString();
					var hojaNueva = libro.Worksheets.Add(tableNueva);
					hojaNueva.ColumnsUsed().AdjustToContents();
				}
				//aqui finaliza el agregar tanque por tanque
				using var memoria = new MemoryStream();
				libro.SaveAs(memoria);
				var nombreExcel = "Reporte.xlsx";
				return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");//Para excel

				//var archivo = File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
				//return archivo;
			}
			catch (Exception)
			{
				throw;

			}
		}



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

		[HttpGet]
		[Route("formatocargue/{filtro}")]
		public IActionResult ExportExcel(string filtro)
		{

			try
			{
				using (var workbook = new XLWorkbook(@"C:\Users\Dagoberto\Documents\ControWell\Client\wwwroot\FormatoCarrotanquesCargados.xlsx"))
				{
					var consulta = JsonSerializer.Deserialize<Consulta>(filtro);
					var RegistrosAExportar = _context.FormatoCargas.Include(o => o.OfertaDiaria).Include(r => r.OfertaDiaria.Ruta).Include(c => c.OfertaDiaria.Conductor).Include(e => e.OfertaDiaria.Empresa).Where(x => x.FechaEnturne >= consulta.FechaInicial && x.FechaInicioTransito <= consulta.FechaFinal).ToList();
					var SampleSheet = workbook.Worksheets.Where(x => x.Name == "SLO-F-028").First();
					int NumeroFila = 5;
					int Item = 1;
					foreach (var i in RegistrosAExportar)
					{
						string Crear = "A" + Convert.ToString(NumeroFila);
						string DocDeTransporte = "B" + Convert.ToString(NumeroFila);
						string OrdenDeCargue = "C" + Convert.ToString(NumeroFila);
						string FechaEnturne = "D" + Convert.ToString(NumeroFila);
						string FechaHoraLlamadoACargar = "E" + Convert.ToString(NumeroFila);
						string FechaHoraInicioLLenado = "F" + Convert.ToString(NumeroFila);
						string FechaHoraFinLLenado = "G" + Convert.ToString(NumeroFila);
						string HoraSalida = "H" + Convert.ToString(NumeroFila);
						string Origen = "I" + Convert.ToString(NumeroFila);
						string CodigoOrigen = "J" + Convert.ToString(NumeroFila);
						string Destino = "K" + Convert.ToString(NumeroFila);
						string CodigoDestino = "L" + Convert.ToString(NumeroFila);
						string CodigoRuta = "M" + Convert.ToString(NumeroFila);
						string ProductoCardado = "N" + Convert.ToString(NumeroFila);
						string CodigoProducto = "O" + Convert.ToString(NumeroFila);
						string Cedula = "P" + Convert.ToString(NumeroFila);
						string NombreConductor = "Q" + Convert.ToString(NumeroFila);
						string Placa = "R" + Convert.ToString(NumeroFila);
						string Tanque = "S" + Convert.ToString(NumeroFila);
						string EmpresaTransporte = "T" + Convert.ToString(NumeroFila);
						string Sellos = "U" + Convert.ToString(NumeroFila);
						string Guia = "V" + Convert.ToString(NumeroFila);
						string Gov = "W" + Convert.ToString(NumeroFila);
						string Gsv = "X" + Convert.ToString(NumeroFila);
						string Neto = "Y" + Convert.ToString(NumeroFila);
						string BSW = "Z" + Convert.ToString(NumeroFila);
						string TEMP = "AA" + Convert.ToString(NumeroFila);
						string API = "AB" + Convert.ToString(NumeroFila);
						string FACTORTEMP = "AC" + Convert.ToString(NumeroFila);
						string Azufre = "AD" + Convert.ToString(NumeroFila);
						string Sal = "AE" + Convert.ToString(NumeroFila);
						string PressVapor = "AF" + Convert.ToString(NumeroFila);
						string IncertidumbreAU = "AG" + Convert.ToString(NumeroFila);
						string IncertidumbreU = "AH" + Convert.ToString(NumeroFila);
						string Observacion = "AI" + Convert.ToString(NumeroFila);
						//*************************************************                     

						SampleSheet.Cell(Crear).Value = "CREAR";
						SampleSheet.Cell(DocDeTransporte).Value = Convert.ToString(Item);
						SampleSheet.Cell(OrdenDeCargue).Value = Convert.ToString(i.OfertaDiaria.DocDeTransporte);
						SampleSheet.Cell(FechaEnturne).Value = Convert.ToString(i.FechaEnturne.ToString("dd/MM/yyyy HH:mm"));
						SampleSheet.Cell(FechaHoraLlamadoACargar).Value = Convert.ToString(i.FechaLlamado.ToString("dd/MM/yyyy HH:mm"));
						SampleSheet.Cell(FechaHoraInicioLLenado).Value = Convert.ToString(i.FechaInicioLlenado.ToString("dd/MM/yyyy HH:mm"));
						SampleSheet.Cell(FechaHoraFinLLenado).Value = Convert.ToString(i.FechaFinLlenado.ToString("dd/MM/yyyy HH:mm"));
						SampleSheet.Cell(HoraSalida).Value = Convert.ToString(i.FechaInicioTransito.ToString("dd/MM/yyyy HH:mm"));
						SampleSheet.Cell(Origen).Value = Convert.ToString("P. NUEVA ESPERANZA");
						SampleSheet.Cell(CodigoOrigen).Value = Convert.ToString(i.OfertaDiaria.Ruta.CodOrigen);
						SampleSheet.Cell(Destino).Value = Convert.ToString(i.OfertaDiaria.Ruta.Destino);
						SampleSheet.Cell(CodigoDestino).Value = Convert.ToString(i.OfertaDiaria.Ruta.CodDestino);
						SampleSheet.Cell(CodigoRuta).Value = Convert.ToString(i.OfertaDiaria.Ruta.CodRuta);
						SampleSheet.Cell(ProductoCardado).Value = Convert.ToString(i.EcopetrolMaterial);
						SampleSheet.Cell(CodigoProducto).Value = Convert.ToString(i.EcopetrolMaterialCod);
						SampleSheet.Cell(Cedula).Value = Convert.ToString(i.OfertaDiaria.Conductor.Cedula);
						SampleSheet.Cell(NombreConductor).Value = Convert.ToString(i.OfertaDiaria.Conductor.Nombre);
						SampleSheet.Cell(Placa).Value = Convert.ToString(i.OfertaDiaria.Placa);
						SampleSheet.Cell(Tanque).Value = Convert.ToString(i.OfertaDiaria.PlacaTanque);
						SampleSheet.Cell(EmpresaTransporte).Value = Convert.ToString(i.OfertaDiaria.Empresa.Nombre);
						SampleSheet.Cell(Sellos).Value = Convert.ToString(i.Sellos);
						SampleSheet.Cell(Guia).Value = Convert.ToString(i.NumGuia);
						SampleSheet.Cell(Gov).Value = Convert.ToString(Math.Round(i.GovCarga, 2));
						SampleSheet.Cell(Gsv).Value = Convert.ToString(Math.Round(i.GsvCarga, 2));
						SampleSheet.Cell(Neto).Value = Convert.ToString(Math.Round(i.NsvCarga, 2));
						SampleSheet.Cell(BSW).Value = Convert.ToString(Math.Round(i.BSWCarga, 4));
						SampleSheet.Cell(TEMP).Value = Convert.ToString(Math.Round(i.TempCarga, 1));
						SampleSheet.Cell(API).Value = Convert.ToString(Math.Round(i.APICarga, 1));
						SampleSheet.Cell(FACTORTEMP).Value = Convert.ToString(Math.Round(i.FactorTempCarga, 5));
						SampleSheet.Cell(Azufre).Value = Convert.ToString(" ????");
						SampleSheet.Cell(Sal).Value = Convert.ToString(" ????");
						SampleSheet.Cell(PressVapor).Value = Convert.ToString(" ????");
						SampleSheet.Cell(IncertidumbreAU).Value = Convert.ToString(" ????");
						SampleSheet.Cell(IncertidumbreU).Value = Convert.ToString(" ????");
						SampleSheet.Cell(Observacion).Value = Convert.ToString(" ");
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
