using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class StatusObject
    {
        public string Statusnaam { get; set; }
        public object Obj { get; set; }

        public StatusObject(string statusnaam, object obj)
        {
            Statusnaam = statusnaam;
            Obj = obj;
        }

        public Werknemer GeefWerknemerObject()
        {
            if (Obj is Werknemer werknemer)
            {
                return werknemer;
            }
            else
            {
                throw new StatusObjectException("Object is geen werknemer");
            }
        }

        public Bedrijf GeefBedrijfObject()
        {
            if (Obj is Bedrijf bedrijf)
            {
                return bedrijf;
            }
            else
            {
                throw new StatusObjectException("Object is geen werknemer");
            }
        }

        public Afspraak GeefAfspraakObject()
        {
            if (Obj is Afspraak afspraak)
            {
                return afspraak;
            }
            else
            {
                throw new StatusObjectException("Object is geen werknemer");
            }
        }
    }
}
