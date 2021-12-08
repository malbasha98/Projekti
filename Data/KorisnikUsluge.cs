using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kucni_poslovi.Data
{
    //[Authorize(Roles = "KorisnikUsluge")]
    public class KorisnikUsluge : RegistrovaniKorisnik
    {
        public double KoordinataSirina { get; set; }
        public double KoordinataDuzina { get; set; }
        public List<Oglas> MojiOglasi { get; set; }

        public override void OceniOglas(int ocena)
        {
            int count = 0; //da li treba 0 ili 1, da li se odmah u oglasu pamti ocena
            foreach (var oglas in MojiOglasi)
            {
                if (oglas.Stanje == Stanje.Izvršen || oglas.Stanje == Stanje.Neaktivan)
                {
                    if (oglas.OcenjenKorisnik != null)
                        count++;
                }
            }
            ProsecnaOcena = ((count-1) * ProsecnaOcena + ocena) / count; //ili sa count
        }


        public KorisnikUsluge()
        {
            MojiOglasi = new List<Oglas>();
        }
    }
}
