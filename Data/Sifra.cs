using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kucni_poslovi.Data
{
    public class Sifra
    {
        [Required]
        public string trenutnaSifra { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string novaSifra { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(novaSifra), ErrorMessage = "Unete šifre se ne poklapaju")]
        public string opetNovaSifra { get; set; }
        public string errorZaSifre { get; set; } = "";

        public string userName { get; set; }
    }
}
