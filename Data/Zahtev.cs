using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kucni_poslovi.Data
{
    public class Zahtev
    {
        [Key]
        public Guid Id { get; set; }
        public bool Pregledan { get; set; }
        public string Naslov { get; set; }
        public Oglas Oglas { get; set; }
        public PruzalacUsluge Ucesnik { get; set; }

        public Zahtev()
        {
            this.Id = Guid.NewGuid();
        }
        
    }
}
