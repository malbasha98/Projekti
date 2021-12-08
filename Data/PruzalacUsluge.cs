using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kucni_poslovi.Data
{
    //[Authorize(Roles = "PruzalacUsluge")]
    public class PruzalacUsluge : RegistrovaniKorisnik
    {

        public TipUsluge TipUsluga { get; set; }
        public List<Oglas> PreuzetOglas { get; set; }
        public List<Zahtev> MojiZahtevi { get; set; }
        public List<EvidencijaOglasa> Evidencije { get; set; }


        public override void OceniOglas(int ocena)
        {
            int count = 0; //da li treba 0 ili 1, da li se odmah u oglasu pamti ocena
            foreach (var oglas in PreuzetOglas)
            {
                if (oglas.Stanje == Stanje.Izvršen || oglas.Stanje == Stanje.Neaktivan)
                {
                    if (oglas.OcenjenPruzalac != null)
                        count++;
                }
            }
            ProsecnaOcena = ((count-1) * ProsecnaOcena + ocena) / count; //ili sa count
        }
    }
}
