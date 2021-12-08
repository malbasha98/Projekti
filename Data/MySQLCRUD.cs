using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kucni_poslovi.Data
{
    public class MySqlCRUD : IdentityDbContext
    {
        public DbSet<KorisnikUsluge> KorisniciUsluga { get; set; }
        public DbSet<PruzalacUsluge> PruzaociUsluga { get; set; }
        public DbSet<Oglas> Oglasi { get; set; }
        public DbSet<Zahtev> Zahtevi { get; set; }
        public DbSet<EvidencijaOglasa> EvidencijeOglasa { get; set; }
        public MySqlCRUD(DbContextOptions<MySqlCRUD> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "KorisnikUsluge",
                    NormalizedName = "KORISNIKUSLUGE"
                });
            builder.Entity<IdentityRole>().HasData(
               new IdentityRole
               {
                   Name = "PruzalacUsluge",
                   NormalizedName = "PRUZALACUSLUGE"
               });
        }


        public async Task<Oglas> nadjiOglas(Guid id)
        {
            return await this.Oglasi
               .Include(x => x.KorisnikUsluge)
               .Include(x => x.Evidencije)
               .ThenInclude(x => x.PruzalacUsluge)
               .Include(x => x.Zahtevi).ThenInclude(x => x.Ucesnik)
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> PostojiZahtevKaPruzaocu(string userName, string idPruzaoca, Guid idOglasa)
        {
            Zahtev postojiZahtev = await this.Zahtevi.
                FirstOrDefaultAsync(x => x.Ucesnik.Id == idPruzaoca && x.Oglas.Id == idOglasa && x.Naslov == userName);
            if (postojiZahtev != null)
                return true;
            else
                return false;
        }

        public async Task<KorisnikUsluge> VratiKorisnika(string userName)
        {
            var Korisnik = await this.KorisniciUsluga
                .Include(x => x.MojiOglasi).ThenInclude(x => x.Zahtevi).ThenInclude(x => x.Ucesnik)
                .Include(x => x.MojiOglasi).ThenInclude(x => x.Zahtevi).ThenInclude(x => x.Oglas).ThenInclude(x => x.KorisnikUsluge)
                .Include(x => x.MojiOglasi).ThenInclude(x => x.Evidencije).ThenInclude(x => x.PruzalacUsluge)
                .Include(x => x.MojiOglasi).ThenInclude(x => x.OdabranPruzalac)
                .Include(x => x.MojiOglasi).ThenInclude(x => x.KorisnikUsluge)
                .FirstOrDefaultAsync(x => x.UserName == userName);

            return Korisnik;
        }

        public async Task<KorisnikUsluge> VratiKorisnikaSaId(string idKorisnika)
        {
            var Korisnik = await this.KorisniciUsluga
                .Include(x => x.MojiOglasi)
                .ThenInclude(x => x.Zahtevi).ThenInclude(x => x.Ucesnik)
                .Include(x => x.MojiOglasi)
                .ThenInclude(x => x.Zahtevi).ThenInclude(x => x.Oglas)
                .Include(x => x.MojiOglasi).ThenInclude(x => x.Evidencije).ThenInclude(x => x.PruzalacUsluge)
                .FirstOrDefaultAsync(x => x.Id == idKorisnika);

            return Korisnik;
        }

        public async Task<bool> DaLiImaKorisnikOglas(string idKorisnika, string idOglasa)
        {
            var Korisnik = await KorisniciUsluga
                            .Include(x => x.MojiOglasi)
                            .FirstOrDefaultAsync(x => x.Id == idKorisnika);
            foreach (var oglas in Korisnik.MojiOglasi)
            {
                if (oglas.Id.ToString() == idOglasa)
                    return true;
            }
            return false;
        }

        public async Task<PruzalacUsluge> VratiPruzaoca(string userName)
        {
            var Pruzalac = await PruzaociUsluga
                            .Include(x => x.Evidencije).ThenInclude(x => x.Oglas).ThenInclude(x => x.KorisnikUsluge)
                            .Include(x => x.PreuzetOglas).ThenInclude(x => x.KorisnikUsluge)
                            .Include(x => x.PreuzetOglas).ThenInclude(x => x.OdabranPruzalac)
                            .Include(x => x.MojiZahtevi).ThenInclude(x => x.Oglas).ThenInclude(x => x.KorisnikUsluge)
                            .Include(x => x.MojiZahtevi).ThenInclude(x => x.Ucesnik)
                            .FirstOrDefaultAsync(x => x.UserName == userName);
            return Pruzalac;
        }

        public async Task<PruzalacUsluge> VratiPruzaocaSaId(string idPruzaoca)
        {
            var Pruzalac = await PruzaociUsluga
                            .Include(x => x.Evidencije).ThenInclude(x => x.Oglas).ThenInclude(x => x.KorisnikUsluge)
                            .Include(x => x.PreuzetOglas)
                            .Include(x => x.MojiZahtevi).ThenInclude(x => x.Oglas)
                            .FirstOrDefaultAsync(x => x.Id == idPruzaoca);
            return Pruzalac;
        }

        public async Task<bool> PostojiZahtevKaKorisniku(string idPruzaoca, Guid idOglasa)
        {
            var Pruzalac = await PruzaociUsluga.FindAsync(idPruzaoca);
            Zahtev postojiZahtev = await this.Zahtevi.
                FirstOrDefaultAsync(x => x.Oglas.Id == idOglasa && x.Naslov == Pruzalac.UserName);
            if (postojiZahtev != null)
                return true;
            else
                return false;
        }

        public string NadjiKorisnika(string userName, out int count, out string slika)
        {
            var Korisnik = this.KorisniciUsluga
                .Include(x => x.MojiOglasi).ThenInclude(x => x.Zahtevi)
                .FirstOrDefault(x => x.UserName == userName);
            count = 0;
            foreach (var oglas in Korisnik.MojiOglasi)
            {
                if (oglas.ProveriDaLiVazi())
                {
                    Update(oglas);
                    SaveChanges();
                }
                foreach (var zahtev in oglas.Zahtevi)
                {
                    if (oglas.Stanje != Stanje.Aktivan)
                    {
                        if (!zahtev.Pregledan)
                        {
                            zahtev.Pregledan = true;
                            Update(zahtev);
                            SaveChanges();
                        }
                    }
                    if (zahtev.Naslov != Korisnik.UserName && !zahtev.Pregledan)
                        count++;
                }
            }
            slika = Korisnik.PutanjaDoSlike;
            return Korisnik.Id;
        }

        public string NadjiPruzaoca(string userName, out int count, out string slika)
        {
            var Pruzalac = PruzaociUsluga
                            .Include(x => x.MojiZahtevi).ThenInclude(x => x.Oglas)
                            .FirstOrDefault(x => x.UserName == userName);
            count = 0;
            foreach (var zahtev in Pruzalac.MojiZahtevi)
            {
                if (zahtev.Oglas.ProveriDaLiVazi())
                {
                    Update(zahtev.Oglas);
                    SaveChanges();
                }
                if (zahtev.Oglas.Stanje != Stanje.Aktivan)
                {
                    if (!zahtev.Pregledan)
                    {
                        zahtev.Pregledan = true;
                        Update(zahtev);
                        SaveChanges();
                    }
                }
                if (zahtev.Naslov != Pruzalac.UserName && !zahtev.Pregledan)
                    count++;
            }
            slika = Pruzalac.PutanjaDoSlike;
            return Pruzalac.Id;
        }

        public async Task ObrisiOglas(Guid idOglasa)
        {
            var oglas = await nadjiOglas(idOglasa);
            List<EvidencijaOglasa> evidencije = await EvidencijeOglasa.Where(x => x.Oglas.Id == oglas.Id).ToListAsync();
            List<Zahtev> zahtevi = await Zahtevi.Where(x => x.Oglas.Id == oglas.Id).ToListAsync();
            if (oglas.PutanjeDoSlika != null)
            {
                var slike = oglas.PutanjeDoSlika.Split('?');
                foreach (var slika in slike)
                {
                    if (File.Exists(@"wwwroot/" + slika))
                        File.Delete(@"wwwroot/" + slika);
                }
            }
            foreach (var ev in evidencije)
            {
                EvidencijeOglasa.Remove(ev);
            }
            foreach (var z in zahtevi)
            {
                Zahtevi.Remove(z);
            }
            Oglasi.Remove(oglas);
            await SaveChangesAsync();
        }

        public async Task<EvidencijaOglasa> nadjiEvidenciju(Guid idOglasa, string idPruzaoca)
        {
            return await this.EvidencijeOglasa
                .Include(x => x.Oglas)
                .Include(x => x.PruzalacUsluge)
                .FirstOrDefaultAsync(x => x.Oglas.Id == idOglasa && x.PruzalacUsluge.Id == idPruzaoca);
        }

        public async Task<Zahtev> nadjiZahtev(Guid idZahteva)
        {
            return await this.Zahtevi
                .Include(x => x.Oglas).ThenInclude(x => x.KorisnikUsluge)
                .Include(x => x.Ucesnik)
                .FirstOrDefaultAsync(x => x.Id == idZahteva);
        }

        public async Task<Zahtev> vratiZahtev(string idPruzaoca, Guid idOglasa)
        {
            return await this.Zahtevi
                .Include(x => x.Oglas)
                .Include(x => x.Ucesnik)
                .FirstOrDefaultAsync(x => x.Ucesnik.Id == idPruzaoca && x.Oglas.Id == idOglasa);
        }

        public async Task DodajEvidencijePruzaocu(string userName)
        {
            var Pruzalac = await PruzaociUsluga.FirstOrDefaultAsync(x => x.UserName == userName);
            foreach (var oglas in Oglasi)
            {
                if (Pruzalac.TipUsluga.HasFlag(oglas.TipUsluge))
                {
                    bool uslov = oglas.Stanje == Stanje.Neaktivan;
                    uslov = uslov && oglas.OdabranPruzalac == null;

                    if (oglas.Stanje == Stanje.Aktivan || uslov)
                    {
                        EvidencijaOglasa novaEvidencija = new EvidencijaOglasa()
                        {
                            Oglas = oglas,
                            Prijavljen = false,
                            PruzalacUsluge = Pruzalac
                        };
                        EvidencijeOglasa.Add(novaEvidencija);
                    }
                }
            }
            await SaveChangesAsync();
        }

    }

}
