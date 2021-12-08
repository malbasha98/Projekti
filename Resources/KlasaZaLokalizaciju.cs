using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Resources;

namespace Kucni_poslovi.Resources
{
    public class KlasaZaLokalizaciju : ISyncfusionStringLocalizer
    {
        public string Get(string key)
        {
            return this.Manager.GetString(key);
        }

        public System.Resources.ResourceManager Manager
        {
            get
            {
                
                return Kucni_poslovi.Resources.SfResources.ResourceManager;
            }
        }
    
}
}
