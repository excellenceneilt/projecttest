﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Distrito
    {
        public int IdDistrito { get; set; }
        public string Descripcion { get; set; }
        public int IdProvincia { get; set; }
        public bool Estado { get; set; }
    }
}
