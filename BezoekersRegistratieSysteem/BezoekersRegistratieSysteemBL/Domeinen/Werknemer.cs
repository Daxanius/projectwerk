using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System.Linq;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class Werknemer : Persoon {

        private Dictionary<Bedrijf, List<string>> _functiePerBedrijf = new Dictionary<Bedrijf, List<string>>();

        /// <summary>
        /// Constructor
        /// </summary>
        public Werknemer() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        /// <param name="email"></param>
        public Werknemer(string voornaam, string achternaam, string email) : base(voornaam, achternaam, email) {
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        /// <param name="email"></param>
        public Werknemer(uint id, string voornaam, string achternaam, string email) : base(id, voornaam, achternaam, email)
        {
        }

        /// <summary>
        /// Voegt bedrijf en functie toe aan de werknemer.
        /// de werknemer word aan een bedrijf toegevoegd als hij/zij nog niet gekend is in het bedrijf met functie.
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <param name="functie"></param>
        /// <exception cref="WerknemerException"></exception>
        public void VoegBedrijfEnFunctieToeAanWerknemer(Bedrijf bedrijf, string functie) {
            if (bedrijf == null) throw new WerknemerException("Werknemer - ZetBedrijf - bedrijf mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Werknemer - ZetBedrijf - functie mag niet leeg zijn");
            if (_functiePerBedrijf.ContainsKey(bedrijf))
            {
                if (!_functiePerBedrijf[bedrijf].Contains(functie))
                {
                    _functiePerBedrijf[bedrijf].Add(functie);
                }
                else throw new WerknemerException("Werknemer - ZetBedrijf - werknemer is in dit bedrijf al werkzaam onder deze functie");
            }
            else
            {
				// Als dit bedrijf deze werknemer nog niet bevat, voeg deze dan toe aan het bedrijf
				if (!bedrijf.GeefWerknemers().Contains(this)) bedrijf.VoegWerknemerToeInBedrijf(this, functie);
                _functiePerBedrijf.Add(bedrijf, new List<string> { functie });
            }
        }

        /// <summary>
        /// Verwijdert bedrijf en functie van de werknemer.
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <exception cref="WerknemerException"></exception>
        public void VerwijderBedrijfVanWerknemer(Bedrijf bedrijf) {
            if (bedrijf == null) throw new WerknemerException("Werknemer - VerwijderBedrijf - bedrijf mag niet leeg zijn");
            if (!_functiePerBedrijf.Keys.Contains(bedrijf)) throw new WerknemerException("Werknemer - VerwijderBedrijf - bedrijf bevat deze werknemer niet");
            // Dit word gebruikt als we een werknemer uit bedrijf halen
            // hierdoor is Bedrijf nullable.
            if (bedrijf.GeefWerknemers().Contains(this)) bedrijf.VerwijderWerknemerUitBedrijf(this);
            _functiePerBedrijf.Remove(bedrijf);
		}

        /// <summary>
        /// Past functie van de werknemer aan.
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
        /// Verwijdert functie van de werknemer
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

        public IReadOnlyDictionary<Bedrijf, List<string>> GeefBedrijfEnFunctiesPerWerknemer()
        {
            return _functiePerBedrijf;
        }
    }
}