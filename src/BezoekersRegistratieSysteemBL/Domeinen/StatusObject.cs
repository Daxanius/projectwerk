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
            if (Obj.GetType() == typeof(Werknemer))
            {
                return (Werknemer)Obj;
            }
            else
            {
                throw new Exception("Object is geen werknemer");
            }
        }

        public Bedrijf GeefBedrijfObject()
        {
            if (Obj.GetType() == typeof(Bedrijf))
            {
                return (Bedrijf)Obj;
            }
            else
            {
                throw new Exception("Object is geen bedrijf");
            }
        }

        public Afspraak GeefAfspraakObject()
        {
            if (Obj.GetType() == typeof(Afspraak))
            {
                return (Afspraak)Obj;
            }
            else
            {
                throw new Exception("Object is geen afspraak");
            }
        }
    }
}
