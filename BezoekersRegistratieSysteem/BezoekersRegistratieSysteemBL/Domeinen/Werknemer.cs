using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class Werknemer : Persoon {
		public Bedrijf? Bedrijf { get; private set; }
		public string Functie { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
		/// <param name="functie"></param>
		public Werknemer(string voornaam, string achternaam, string email, Bedrijf bedrijf, string functie) : base(voornaam, achternaam, email) {
			ZetBedrijf(bedrijf);
			ZetFunctie(functie);
		}

		/// <summary>
		/// Past bedrijf aan,
		/// de werknemer word aan bedrijf toegevoegd als bedrijf
		/// deze werknemer niet bevat
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <exception cref="WerknemerException"></exception>
		public void ZetBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new WerknemerException("Werknemer - ZetBedrijf - bedrijf mag niet leeg zijn");
            if (Bedrijf != null) Bedrijf.VerwijderWerknemer(this);

			// Als dit bedrijf niet al deze werknemer bevat, voeg deze dan toe aan het bedrijf
			if (!bedrijf.GeefWerknemers().Contains(this)) bedrijf.VoegWerknemerToe(this);
			Bedrijf = bedrijf;
		}
		
		/// <summary>
		/// Zet bedrijf op null
		/// </summary>
		public void VerwijderBedrijf() {
			// Dit word gebruikt als we een werknemer uit bedrijf halen
			// hierdoor is Bedrijf nullable.
			Bedrijf = null;
		}

		/// <summary>
		/// Past functie aan
		/// </summary>
		/// <param name="functie"></param>
		/// <exception cref="WerknemerException"></exception>
		public void ZetFunctie(string functie) {
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Werknemer - ZetFunctie - functie mag niet leeg zijn");
            Functie = functie;
		}
	}
}