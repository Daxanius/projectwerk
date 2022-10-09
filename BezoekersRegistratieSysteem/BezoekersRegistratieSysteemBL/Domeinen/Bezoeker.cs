using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	/// <summary>
	/// Informatie over bezoekers
	/// </summary>
	public class Bezoeker : Persoon {
		public string Bedrijf { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
		public Bezoeker(string voornaam, string achternaam, string email, string bedrijf) : base(voornaam, achternaam, email) {
			ZetBedrijf(bedrijf);
		}

		/// <summary>
		/// Past het bedrijf aan
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <exception cref="BezoekerException"></exception>
		public void ZetBedrijf(string bedrijf) {
			if (string.IsNullOrWhiteSpace(bedrijf)) throw new BezoekerException("Bedrijf mag niet leeg zijn");
			Bedrijf = bedrijf;
		}
	}
}