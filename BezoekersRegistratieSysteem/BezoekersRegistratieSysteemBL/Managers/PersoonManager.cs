using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Managers
{
    public class PersoonManager
    {
        public void VoegPersoonToe(IPersoon persoon)
        {

        }

        public void VerwijderPersoon(IPersoon persoon)
        {

        }

        public void BewerkPersoon(IPersoon persoon)
        {

        }

        public IReadOnlyList<Werknemer> GeefWerknemers()
        {
            return null;
        }

        public IReadOnlyList<Werknemer> GeefWerknemerPerBedrijf(Bedrijf bedrijf)
        {
            return null;
        }

        public IReadOnlyList<Werknemer> GeefWerknemerPerFunctie(string functie)
        {
            return null;
        }

        public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers(DateTime tijdstip)
        {
            return null;
        }

        public IReadOnlyList<Bezoeker> GeefBezoekersPerBedrijf(Bedrijf bedrijf)
        {
            return null;
        }

        public IReadOnlyList<Bezoeker> GeefBezoekersPerDatum(DateTime datum)
        {
            return null;
        }
    }
}
