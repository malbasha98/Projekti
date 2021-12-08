using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kucni_poslovi.Data
{
    public class EvidencijaOglasa
    {
        [Key]
        public Guid Id { get; set; }
        public bool Prijavljen { get; set; }
        public Oglas Oglas { get; set; }
        public PruzalacUsluge PruzalacUsluge { get; set; }

    }
}
