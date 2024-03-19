using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControWell.Shared
{
    public class FormatoDescargueNaf
    {
        public int Id { get; set; }
        public string DocTr { get; set; }=string.Empty;
        public DateTime FinTransito { get; set; }=DateTime.Now;
        public DateTime LlamadoDescargue { get; set; } = DateTime.Now;
        public DateTime InicioEntrega { get; set; } = DateTime.Now;
        public DateTime FinEntrega { get; set; } = DateTime.Now;
        public Ruta? Ruta { get; set; }  
        public int RutaId { get; set; }  
        public string MaterialDesc { get; set; } = string.Empty;
        public string CodMaterial { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string NombreConductor { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string Tanque { get; set; } = string.Empty;
        public string EmpresaTr { get; set; } = string.Empty;
        public string Sellos { get; set; } = string.Empty;
        public string Guia { get; set; } = string.Empty;
        public double GovDescarga { get; set; } 
        public double GsvDescarga { get; set; } 
        public double NsvDescarga { get; set; } 
        public double BswDescarga { get; set; } 
        public double TempDescarga { get; set; } 
        public double Api60Descarga { get; set; }         
        public double FactorTempDescarga { get; set; }       
        public double IncertidumbreExpaPorcentDescarga { get; set; }       
        public double IncertidumbreExpaBslDescarga { get; set; } 
        public string Observaciones { get; set; } = string.Empty;
        public string Gut2 { get; set; } = string.Empty;
    }
}
