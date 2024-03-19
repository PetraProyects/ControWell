using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class PruebasProduccion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }=string.Empty;
        public DateTime FechaInicio { get; set; }=DateTime.Now;
        public DateTime FechaFin { get; set; }=DateTime.Now;
        public double? Horas {  get; set; }  
        public Pozo? Pozo { get; set; }
        public int PozoId { get; set; }
        public Tanque? Tanque { get; set; }
        public int TanqueId { get;set; }
    }
}
