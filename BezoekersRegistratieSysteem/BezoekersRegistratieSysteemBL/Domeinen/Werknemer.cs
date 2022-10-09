using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class Werknemer : Persoon {
		public Bedrijf? Bedrijf { get; private set; }
		public string Functie { get; private set; }

		public Werknemer(string voornaam, string achternaam, string email, Bedrijf bedrijf, string functie) : base(voornaam, achternaam, email) {
			ZetBedrijf(bedrijf);
			ZetFunctie(functie);
		}

		public void ZetBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null) throw new WerknemerException("Bedrijf mag niet leeg zijn");
			if (Bedrijf != null) Bedrijf.VerwijderWerknemer(this);

			// Als dit bedrijf niet al deze werknemer bevat, voeg deze dan toe aan het bedrijf
			if (!bedrijf.GeefWerknemers().Contains(this)) bedrijf.VoegWerknemerToe(this);
			Bedrijf = bedrijf;
		}

		public void VerwijderBedrijf() {
			// Dit word gebruikt als we een werknemer uit bedrijf halen
			// hierdoor is Bedrijf nullable. Kunnen werknemers zonder bedrijf zitten?
			Bedrijf = null;
		}

		public void ZetFunctie(string functie) {
			if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Functie mag niet leeg zijn");
			Functie = functie;
		}
	}
}