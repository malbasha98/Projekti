using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kucni_poslovi.Data
{

    [Flags]
    public enum TipUsluge
    {
        Dadilje = 1,
        Negovatelji = 2,
        KućniPomoćnici = 4,
        ČuvariKućnihLjubimaca = 8,
        Baštovani = 16
    }
}
