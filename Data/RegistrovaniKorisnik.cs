using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Identity;

namespace Kucni_poslovi.Data
{
    public abstract class RegistrovaniKorisnik : IdentityUser
    {

        [Required]
        [Display(Name = "Korisničko ime")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Display(Name = "Datum rodjenja")]
        [DataType(DataType.Date)]
        public DateTime DatumRodjenja { get; set; }
        public string PutanjaDoSlike { get; set; }
        public double ProsecnaOcena { get; set; }
        public string Opis { get; set; }
        public abstract void OceniOglas(int ocena);

        

    }
}
