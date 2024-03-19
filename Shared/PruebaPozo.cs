using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class PruebaPozo
    {
        public int Id { get; set; }
        //pozo
        public Pozo? Pozo { get; set; }
        public int PozoId { get; set; }
        //Datos de cabeza de pozo
        public double? WhpPsig { get; set; }
        public double? WhtF { get; set; }
        //Datos de variador
        public double? FrecuenciaHzPumpSpeedRpm { get; set; }
        public double? PipPsigPumpTorqueLbFt { get; set; }
        public double? PdpPsigTorquePorcent { get; set; }
        public double? TempMotorF { get; set; }
        public double? TempInTakeF { get; set; }
        public double? Amp { get; set; }
        public double? Volt { get; set; }
        //Datos laboratorio
        public double? ApiCabeza60F { get; set; }
        public double? ApiMezcla60F { get; set; }
        public double? SywCabezaPorcent { get; set; }
        public double? SywMezclaPorcent { get; set; }
        public double? ClorurosPpm { get; set; }
        public double? Ph { get; set; }
        //Tasa Inyeccion
        public double? TasaInyNafSuperficieBpd { get; set; }
        public double? TasaInyPorCapilarBpd { get; set; }
        public double? PresInyNafPorCapilarPsi { get; set; }
        //Inyeccion quimica
        public double? InhibidorCm { get; set; }
        public double? RompedorCm { get; set; }
        //tiempo
        public int Horas {  get; set; }
        public DateTime FechaInicio {  get; set; }=DateTime.Now;
        public DateTime FechaFin {  get; set; }=DateTime.Now;

    }
}
