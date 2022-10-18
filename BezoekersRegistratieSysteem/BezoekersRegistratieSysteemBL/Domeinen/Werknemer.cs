using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System.Linq;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class Werknemer : Persoon {

        private Dictionary<Bedrijf, List<string>> _functiePerBedrijf = new Dictionary<Bedrijf, List<string>>();

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
        public Werknemer(string voornaam, string achternaam, string email) : base(voornaam, achternaam, email) {
		}

        /// <summary>
        /// Constructor voor het aanmaken van een nieuwe werknemer in de Datalaag.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        /// <param name="email"></param>
        public Werknemer(uint id, string voornaam, string achternaam, string email) : base(id, voornaam, achternaam, email)
        {
        }

        /// <summary>
        /// Voegt bedrijf en functie toe aan een werknemer.
        /// Een werknemer word aan een bedrijf toegevoegd als hij/zij nog niet gekend is in het bedrijf met functie.
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <param name="functie"></param>
        /// <exception cref="WerknemerException"></exception>
        public void VoegBedrijfEnFunctieToeAanWerknemer(Bedrijf bedrijf, string functie) {
            if (bedrijf == null) throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - bedrijf mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - functie mag niet leeg zijn");
            if (_functiePerBedrijf.ContainsKey(bedrijf))
            {
                if (!_functiePerBedrijf[bedrijf].Contains(functie))
                {
                    _functiePerBedrijf[bedrijf].Add(functie);
                }
                else throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - werknemer is in dit bedrijf al werkzaam onder deze functie");
            }
            else
            {
                if (!bedrijf.GeefWerknemers().Contains(this))
                {
                    bedrijf.VoegWerknemerToeInBedrijf(this, functie);
                }
                else
                {
                    _functiePerBedrijf.Add(bedrijf, new List<string> { functie });
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
            if (!_functiePerBedrijf.Keys.Contains(bedrijf)) throw new WerknemerException("Werknemer - VerwijderBedrijfVanWerknemer - bedrijf bevat deze werknemer niet");
            // Dit word gebruikt als we een werknemer uit bedrijf halen
            // hierdoor is Bedrijf nullable.
            if (bedrijf.GeefWerknemers().Contains(this)) bedrijf.VerwijderWerknemerUitBedrijf(this);
            _functiePerBedrijf.Remove(bedrijf);
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
            if (!_functiePerBedrijf.ContainsKey(bedrijf)) throw new WerknemerException("Werknemer - WijzigFunctie - bedrijf bevat deze werknemer niet");
            if (!_functiePerBedrijf[bedrijf].Contains(oudeFunctie)) throw new WerknemerException("Werknemer - WijzigFunctie - werknemer is in dit bedrijf niet werkzaam onder deze functie");
            if (_functiePerBedrijf[bedrijf].Contains(nieuweFunctie)) throw new WerknemerException("Werknemer - WijzigFunctie - werknemer is in dit bedrijf al werkzaam onder deze functie");
            _functiePerBedrijf[bedrijf].Remove(oudeFunctie);
            _functiePerBedrijf[bedrijf].Add(nieuweFunctie);
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
            if (!_functiePerBedrijf.ContainsKey(bedrijf)) throw new WerknemerException("Werknemer - VerwijderFunctie - bedrijf bevat deze werknemer niet");
            if (_functiePerBedrijf[bedrijf].Contains(functie))
            {
                _functiePerBedrijf[bedrijf].Remove(functie);
            }
            else throw new WerknemerException("Werknemer - VerwijderFunctie - werknemer is in dit bedrijf niet werkzaam onder deze functie");
        }

        /// <summary>
        /// Geeft bedrijf en functies voor een werknemer terug.
        /// </summary>
        /// <exception cref="WerknemerException"></exception>
        public IReadOnlyDictionary<Bedrijf, List<string>> GeefBedrijfEnFunctiesPerWerknemer()
        {
            return _functiePerBedrijf;
        }

        /// <summary>
        /// Geeft bedrijf voor een werknemer terug.
        /// </summary>
        /// <exception cref="WerknemerException"></exception>
        public Bedrijf HaalBedrijfOp(uint id)
        {
            foreach (var bedrijf in _functiePerBedrijf.Keys)
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
            foreach (Bedrijf bedrijf in _functiePerBedrijf.Keys)
            {
                if (!werknemer._functiePerBedrijf.ContainsKey(bedrijf)) return false;
                foreach (var functie in _functiePerBedrijf[bedrijf])
                {
                    if (!werknemer._functiePerBedrijf[bedrijf].Contains(functie)) return false;
                }
            }
            return true;
        }

    }
}