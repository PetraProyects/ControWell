﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControWell.Shared
{
    public class Tanque
    {
        public int Id { get; set; }
        public string NombreTanque { get; set; } = string.Empty;
        public double? Capacidad { get; set; }
        public string TipoFluido { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public double? TBase { get; set; }
    }
}
