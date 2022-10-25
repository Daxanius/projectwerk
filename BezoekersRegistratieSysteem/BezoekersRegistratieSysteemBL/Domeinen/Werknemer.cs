using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System.Linq;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class Werknemer {

        public uint Id { get; private set; }
        public string Voornaam { get; private set; }
        public string Achternaam { get; private set; }
        public Dictionary <Bedrijf, WerknemerInfo> werknemerInfo { get; private set; }

        /// <summary>
        /// Constructor REST
        /// </summary>
        public Werknemer() { }

        /// <summary>
        /// Constructor voor het aanmaken van een nieuwe werknemer in de BusinessLaag.
        /// </summary>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        /// <param name="email"></param>
        public Werknemer(string voornaam, string achternaam)
        {
            ZetVoornaam(voornaam);
            ZetAchternaam(achternaam);
        }

        /// <summary>
        /// Constructor voor het aanmaken van een nieuwe werknemer in de Datalaag.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        public Werknemer(uint id, string voornaam, string achternaam)
        {
            ZetId(id);
            ZetVoornaam(voornaam);
            ZetAchternaam(achternaam);
        }

        /// <summary>
        /// Zet id werknemer.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="WerknemerException"></exception>
        public void ZetId(uint id)
        {
            Id = id;
        }
        
        /// <summary>
        /// Zet voornaam werknemer.
        /// </summary>
        /// <param name="voornaam"></param>
        /// <exception cref="WerknemerException"></exception>
        public void ZetVoornaam(string voornaam)
        {
            if (string.IsNullOrWhiteSpace(voornaam)) throw new WerknemerException("Werknemer - ZetVoornaam - voornaam mag niet leeg zijn");
            Voornaam = voornaam.Trim();
        }
        
        /// <summary>
        /// Zet achternaam werknemer.
        /// </summary>
        /// <param name="achternaam"></param>
        /// <exception cref="WerknemerException"></exception>
        public void ZetAchternaam(string achternaam)
        {
            if (string.IsNullOrWhiteSpace(achternaam)) throw new WerknemerException("Werknemer - ZetAchternaam - achternaam mag niet leeg zijn");
            Achternaam = achternaam.Trim();
        }

        /// <summary>
        /// Voegt bedrijf en functie toe aan een werknemer.
        /// Een werknemer word aan een bedrijf toegevoegd als hij/zij nog niet gekend is in het bedrijf met functie.
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <param name="functie"></param>
        /// <exception cref="WerknemerException"></exception>
        public void VoegBedrijfEnFunctieToeAanWerknemer(Bedrijf bedrijf, string email, string functie) {
            if (bedrijf == null) throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - bedrijf mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - functie mag niet leeg zijn");
            if (werknemerInfo.ContainsKey(bedrijf))
            {
                if (!werknemerInfo[bedrijf].Functies.Contains(functie))
                {
                    werknemerInfo[bedrijf].Functies.Add(functie);
                }
                else throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - werknemer is in dit bedrijf al werkzaam onder deze functie");
            }
            else
            {
                if (!bedrijf.GeefWerknemers().Contains(this))
                {
                    bedrijf.VoegWerknemerToeInBedrijf(this, email, functie);
                }
                else
                {
                    werknemerInfo.Add(bedrijf, new WerknemerInfo(bedrijf, email));
                    werknemerInfo[bedrijf].Functies.Add(functie);
                }
            }
        }

        /// <summary>
        /// Verwijdert bedrijf en functie van een werknemer.
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <exception cref="WerknemerException"></exception>
        public void VerwijderBedrijfVanWerknemer(Bedrijf bedrijf) {
            if (bedrijf == null) throw new WerknemerException("Werknemer - VerwijderBedrijfVanWerknemer - bedrijf mag niet leeg zijn");
            if (!werknemerInfo.Keys.Contains(bedrijf)) throw new WerknemerException("Werknemer - VerwijderBedrijfVanWerknemer - bedrijf bevat deze werknemer niet");
            // Dit word gebruikt als we een werknemer uit bedrijf halen
            // hierdoor is Bedrijf nullable.
            if (bedrijf.GeefWerknemers().Contains(this)) bedrijf.VerwijderWerknemerUitBedrijf(this);
            werknemerInfo.Remove(bedrijf);
		}

        /// <summary>
        /// Past functie van een werknemer aan.
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <param name="oudeFunctie"></param>
        /// <param name="nieuweFunctie"></param>
        /// <exception cref="WerknemerException"></exception>
        public void WijzigFunctie(Bedrijf bedrijf, string oudeFunctie, string nieuweFunctie)
        {
            if (bedrijf == null) throw new WerknemerException("Werknemer - WijzigFunctie - bedrijf mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(oudeFunctie)) throw new WerknemerException("Werknemer - WijzigFunctie - oude functie mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(nieuweFunctie)) throw new WerknemerException("Werknemer - WijzigFunctie - nieuw functie mag niet leeg zijn");
            if (!werknemerInfo.ContainsKey(bedrijf)) throw new WerknemerException("Werknemer - WijzigFunctie - bedrijf bevat deze werknemer niet");
            if (!werknemerInfo[bedrijf].Functies.Contains(oudeFunctie)) throw new WerknemerException("Werknemer - WijzigFunctie - werknemer is in dit bedrijf niet werkzaam onder deze functie");
            if (werknemerInfo[bedrijf].Functies.Contains(nieuweFunctie)) throw new WerknemerException("Werknemer - WijzigFunctie - werknemer is in dit bedrijf al werkzaam onder deze functie");
            werknemerInfo[bedrijf].Functies.Remove(oudeFunctie);
            werknemerInfo[bedrijf].Functies.Add(nieuweFunctie);
        }

        /// <summary>
        /// Verwijdert functie van een werknemer.
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <param name="functie"></param>
        /// <exception cref="WerknemerException"></exception>
        public void VerwijderFunctie(Bedrijf bedrijf, string functie)
        {
            if (bedrijf == null) throw new WerknemerException("Werknemer - VerwijderFunctie - bedrijf mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Werknemer - VerwijderFunctie - functie mag niet leeg zijn");
            if (!werknemerInfo.ContainsKey(bedrijf)) throw new WerknemerException("Werknemer - VerwijderFunctie - bedrijf bevat deze werknemer niet");
            if (werknemerInfo[bedrijf].Functies.Count() == 1) throw new WerknemerException("Werknemer - VerwijderFunctie - werknemer is in dit bedrijf werkzaam onder deze functie en kan niet verwijderd worden");
            if (werknemerInfo[bedrijf].Functies.Contains(functie))
            {
                werknemerInfo[bedrijf].Functies.Remove(functie);
            }
            else throw new WerknemerException("Werknemer - VerwijderFunctie - werknemer is in dit bedrijf niet werkzaam onder deze functie");
        }

        /// <summary>
        /// Geeft bedrijf en functies voor een werknemer terug.
        /// </summary>
        /// <exception cref="WerknemerException"></exception>
        public IReadOnlyDictionary<Bedrijf, WerknemerInfo> GeefBedrijvenEnFunctiesPerWerknemer()
        {
            return werknemerInfo;
        }

        /// <summary>
        /// Geeft bedrijf voor een werknemer terug.
        /// </summary>
        /// <exception cref="WerknemerException"></exception>
        public Bedrijf HaalBedrijfOp(uint id)
        {
            foreach (var bedrijf in werknemerInfo.Keys)
            {
                if (bedrijf.Id == id) return bedrijf;
            }
            return null;
        }

        /// <summary>
        /// Vergelijkt werknemers op inhoud.
        /// </summary>
        /// <exception cref="BedrijfException"></exception>
        public bool WerknemerIsGelijk(Werknemer werknemer)
        {
            if (werknemer == null) return false;
            if (werknemer.Id != Id) return false;
            if (werknemer.Voornaam != Voornaam) return false;
            if (werknemer.Achternaam != Achternaam) return false;
            foreach (Bedrijf bedrijf in werknemerInfo.Keys)
            {
                if (!werknemer.werknemerInfo.ContainsKey(bedrijf)) return false;
                if (werknemerInfo[bedrijf].Functies.Count() != werknemer.werknemerInfo[bedrijf].Functies.Count()) return false;
                foreach (string functie in werknemerInfo[bedrijf].Functies)
                {
                    if (!werknemer.werknemerInfo[bedrijf].Functies.Contains(functie)) return false;
                }
            }
            return true;
        }

    }
}