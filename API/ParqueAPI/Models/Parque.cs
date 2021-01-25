﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParqueAPI.Models
{
    public class Parque
    {
        public long ParqueID { get; set; }

        public string NomeParque { get; set; }

        public bool ParquePublico { get; set; }

        [ForeignKey("MoradaID")]
        public long MoradaID { get; set; }
        public Morada Morada { get; set; }
    }
}