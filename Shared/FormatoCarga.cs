using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class FormatoCarga
    {
        public int Id { get; set; }
        //public string DocTransporte { get; set; }=string.Empty; Lo trar la oferta diaria
        public string OrdenCargue { get; set; }=string.Empty;//El mismo doc transporte
        public DateTime FechaEnturne { get; set; } = DateTime.Now;
        public DateTime FechaLlamado { get; set; } = DateTime.Now;
        public DateTime FechaInicioLlenado { get; set; } = DateTime.Now;
        public DateTime FechaFinLlenado { get; set; } = DateTime.Now;
        public DateTime FechaInicioTransito { get; set; } = DateTime.Now;
        public string EcopetrolMaterial { get; set; } = string.Empty;        
        public string EcopetrolMaterialCod { get; set; } = string.Empty;        
        public OfertaDiaria? OfertaDiaria { get; set; }
        public int OfertaDiariaId { get; set; }
        public string Sellos { get; set; }= string.Empty;               
        public string NumGuia {  get; set; }= string.Empty;        
        public double GovCarga { get; set; }
        public double GsvCarga { get; set; }
        public double NsvCarga { get; set; }
        public double BSWCarga { get; set; }
        public double TempCarga { get; set; }
        public double APICarga { get; set; }
        public double FactorTempCarga { get; set; }
        public double Azufre { get; set; }
        public double SalPTB { get; set; }
        public double PresVaporReid { get; set; }
        public double IncertiExpandidoAUPorcen { get; set; }
        public double IncertiExpandidoUBls { get; set; }
        public string Observaciones {  get; set; }= string.Empty;        
        public string GUT2 {  get; set; }=string.Empty;
    }
}
