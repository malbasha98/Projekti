using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kucni_poslovi.Data
{
    public enum Stanje
    {
        Aktivan = 0,
        UIzvršavanju,
        Izvršen,
        Neaktivan
    }

    public class Oglas
    {
        
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Opis { get; set; }
        public double? OcenjenKorisnik { get; set; } //ako je null nije ocenjen ako nije null onda je ocena
        public double? OcenjenPruzalac { get; set; } //ako je null nije ocenjen ako nije null onda je ocena
        public Stanje Stanje { get; set; }
        public DateTime DatumVazenja { get; set; }
        public TipUsluge TipUsluge { get; set; }
        public string PutanjeDoSlika { get; set; }
        [Required]
        public string NaslovOglasa { get; set; }

        public bool[,] VremenskaTabela;
        public KorisnikUsluge KorisnikUsluge { get; set; }
        public PruzalacUsluge OdabranPruzalac { get; set; }
        public List<Zahtev> Zahtevi { get; set; }
        public List<EvidencijaOglasa> Evidencije { get; set; }

        public Oglas()
        {
            Id = Guid.NewGuid();
            Zahtevi = new List<Zahtev>();
            Evidencije = new List<EvidencijaOglasa>();
            VremenskaTabela = new bool[24,7];
        }

        public bool ProveriDaLiVazi()
        {
            if (this.Stanje == Stanje.Aktivan)
            {
                if (DateTime.Compare(this.DatumVazenja, DateTime.Now) < 0)
                {
                    this.Stanje = Stanje.Neaktivan;
                    return true;
                }

            }
            return false;
        }

    }
}
