using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;
namespace ControWell.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PruebaPozoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PruebaPozoController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PruebaPozo>>> GetPruebaPozo()
        {
            var lista = await _context.PruebaPozos.Include(p=>p.Pozo).ToListAsync();
            return Ok(lista);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<PruebaPozo>>> GetSinglePruebaPozo(int id)
        {
            var miobjeto = await _context.PruebaPozos.FirstOrDefaultAsync(ob => ob.Id == id);
            if (miobjeto == null)
            {
                return NotFound(" :/");
            }

            return Ok(miobjeto);
        }
        [HttpPost]

        public async Task<ActionResult<PruebaPozo>> CreatePruebaPozo(PruebaPozo objeto)
        {

            _context.PruebaPozos.Add(objeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbPruebaPozo());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<PruebaPozo>>> UpdatePruebaPozo(PruebaPozo objeto)
        {

            var DbObjeto = await _context.PruebaPozos.FindAsync(objeto.Id);
            if (DbObjeto == null)
                return BadRequest("no se encuentra");
            DbObjeto.WhpPsig = objeto.WhpPsig;
            DbObjeto.WhtF = objeto.WhtF;
            DbObjeto.FrecuenciaHzPumpSpeedRpm = objeto.FrecuenciaHzPumpSpeedRpm;
            DbObjeto.PipPsigPumpTorqueLbFt = objeto.PipPsigPumpTorqueLbFt;
            DbObjeto.PdpPsigTorquePorcent = objeto.PdpPsigTorquePorcent;
            DbObjeto.TempMotorF = objeto.TempMotorF;
            DbObjeto.TempInTakeF = objeto.TempInTakeF;
            DbObjeto.Amp = objeto.Amp;
            DbObjeto.Volt = objeto.Volt;
            DbObjeto.ApiCabeza60F = objeto.ApiCabeza60F;
            DbObjeto.SywCabezaPorcent = objeto.SywCabezaPorcent;
            DbObjeto.SywMezclaPorcent = objeto.SywMezclaPorcent;
            DbObjeto.ClorurosPpm = objeto.ClorurosPpm;
            DbObjeto.Ph = objeto.Ph;
            DbObjeto.TasaInyNafSuperficieBpd = objeto.TasaInyNafSuperficieBpd;
            DbObjeto.TasaInyPorCapilarBpd = objeto.TasaInyPorCapilarBpd;
            DbObjeto.PresInyNafPorCapilarPsi = objeto.PresInyNafPorCapilarPsi;
            DbObjeto.InhibidorCm = objeto.InhibidorCm;
            DbObjeto.RompedorCm = objeto.RompedorCm;
            DbObjeto.FechaInicio = objeto.FechaInicio;
            DbObjeto.FechaFin = objeto.FechaFin;
            DbObjeto.Horas = objeto.Horas;
            await _context.SaveChangesAsync();
            return Ok(await _context.PruebaPozos.ToListAsync());
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<PruebaPozo>>> DeletePruebaPozo(int id)
        {
            var DbObjeto = await _context.PruebaPozos.FirstOrDefaultAsync(Ob => Ob.Id == id);
            if (DbObjeto == null)
            {
                return NotFound("no existe :/");
            }

            _context.PruebaPozos.Remove(DbObjeto);
            await _context.SaveChangesAsync();
            return Ok(await GetDbPruebaPozo());
        }


        private async Task<List<PruebaPozo>> GetDbPruebaPozo()
        {
            return await _context.PruebaPozos.ToListAsync();
        }

        [HttpGet]
        [Route("innergy/{filtro}")]
        public IActionResult ExportExcel(string filtro)
        {

            try
            {
                using (var workbook = new XLWorkbook(@"C:\ControWell\Client\wwwroot\NEPLANTILLA.xlsx"))
                {
                    var consulta = JsonSerializer.Deserialize<Consulta>(filtro);
                    var RegistrosAExportar = _context.PruebaPozos.Include(p=>p.Pozo).Where(x=>x.FechaInicio>=consulta.FechaInicial&&x.FechaInicio<=consulta.FechaFinal).ToList();
                    var pozos = _context.Pozos.ToList();
                    var ultimaspruebas=new List<PruebaPozo>();
                    foreach(var p in pozos)
                    {
                        var listpozoprueb= RegistrosAExportar.Where(x=>x.PozoId==p.Id).ToList();
                        if (listpozoprueb.Count() > 0)
                        {
                            var fechaultimo= listpozoprueb.Max(x=>x.FechaInicio);
                            var ultimo= listpozoprueb.Where(x=>x.FechaInicio == fechaultimo).FirstOrDefault();
							ultimaspruebas.Add(ultimo);
						}

					}
                    var SampleSheet = workbook.Worksheets.Where(x => x.Name == "REPORTE").First();                    
                    char[] columnas = {'E','F','G','H','I','J','K','L','M','N' };
                    int letraCol = 0;
                    SampleSheet.Cell("D3").Value = DateTime.Now;
					foreach (var i in ultimaspruebas)
                    {                        
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"7").Value = i.Pozo.NombrePozo;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"9").Value = i.Horas;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"13").Value = i.WhpPsig;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"14").Value = i.WhtF;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"18").Value = i.FrecuenciaHzPumpSpeedRpm;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"19").Value = i.PipPsigPumpTorqueLbFt;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"20").Value = i.PdpPsigTorquePorcent;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"21").Value = i.TempMotorF;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"22").Value = i.TempInTakeF;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"23").Value = i.Amp;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"24").Value = i.Volt;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"28").Value = i.ApiCabeza60F;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"29").Value = i.ApiCabeza60F;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"30").Value = i.SywCabezaPorcent;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"31").Value = i.SywMezclaPorcent;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"32").Value = i.ClorurosPpm;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"33").Value = i.Ph;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"37").Value = i.TasaInyNafSuperficieBpd;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"38").Value = i.TasaInyPorCapilarBpd;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"39").Value = i.PresInyNafPorCapilarPsi;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"43").Value = i.InhibidorCm;
                        SampleSheet.Cell(Convert.ToString(columnas[letraCol])+"44").Value = i.RompedorCm;

						letraCol+=1;
					}
                    //RESUMEN DE PRODUCCION
                    var movimientos=_context.Balances.Include(t=>t.Tanque).Where(x=>x.Fecha>=consulta.FechaInicial&&x.Fecha<=consulta.FechaFinal).ToList();
                    double tov = 0;
                    double gsv = 0;
                    double syw = 0;
                    double nsv = 0;
                    double aguaPr = 0;
                    double gas = 0;
                    double NafIny = 0;
                    foreach(var i in movimientos)
                    {
                        if (i.Tanque.TipoFluido == "Crudo" && i.TipoMovimiento == "Produccion")
                        {
                            tov += (double)i.DeltaTov;
                            gsv += (double)i.DeltaGsv;
                            nsv += (double)i.DeltaNsv;
                            aguaPr += (double)i.DeltaAguaNeta;
                        }
                        if (i.Tanque.TipoFluido == "Refinado" && i.TipoMovimiento == "Consumo")
                        {
                            NafIny += (-1)*(double)i.DeltaNsv;
						}
                    }
					SampleSheet.Cell("E53").Value = tov;
					SampleSheet.Cell("E54").Value = gsv;
					SampleSheet.Cell("E55").Value = syw;
					SampleSheet.Cell("E56").Value = nsv;
					SampleSheet.Cell("E57").Value = aguaPr;
					SampleSheet.Cell("E58").Value = gas;
					SampleSheet.Cell("E59").Value = NafIny;
                    //STOCK EN TANQUES
                    var tanques=_context.Tanques.ToList();
					var movimientosAnte = _context.Balances.Include(t => t.Tanque).Where(x => x.Fecha <= consulta.FechaFinal).ToList();
                    var losultMov=new List<Balance>();
                    foreach(var i in tanques)
                    {
                        var movTan= movimientosAnte.Where(x=>x.TanqueId==i.Id).ToList();
                        if (movTan.Count() > 0)
                        {
                            var MovMaxFe = movTan.Max(x => x.Fecha);
                            var movIndTan= movTan.Where(x=>x.Fecha== MovMaxFe).FirstOrDefault();
                            losultMov.Add(movIndTan);
						}
					}
                    double stockGsv = 0;
                    double stockNsv = 0;
                    double stockAgua = 0;
                    double tovNaf = 0;
                    double nsvNaf = 0;
                    double apiPondIny = 0;
                    foreach(var i in losultMov)
                    {
                        if (i.Tanque.TipoFluido == "Crudo")
                        {
							stockGsv += i.GSV();
							stockNsv += i.NSV();
							stockAgua += i.AGUANETA();
						}
						if (i.Tanque.TipoFluido == "Refinado")
						{
							tovNaf += (double)i.Tov;
							nsvNaf += i.NSV();
                            apiPondIny = i.Api60F();//???
						}

					}
					SampleSheet.Cell("E64").Value = Math.Round(stockGsv,2);
					SampleSheet.Cell("E65").Value = Math.Round(stockNsv,2);
					SampleSheet.Cell("E66").Value = Math.Round(stockAgua,2);
					SampleSheet.Cell("K64").Value = Math.Round(tovNaf, 2);
					SampleSheet.Cell("K65").Value = Math.Round(nsvNaf, 2);
					SampleSheet.Cell("K66").Value = Math.Round(apiPondIny, 2);
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
