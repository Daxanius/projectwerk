using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	/// <summary>
	/// Informatie over bezoekers
	/// </summary>
	public class Bezoeker : Persoon {
		public string Bedrijf { get; private set; }

        /// <summary>
		/// Constructor REST
		/// </summary>
		public Bezoeker() { }

        /// <summary>
        /// Constructor voor het aanmaken van een bezoeker in de BusinessLaag.
        /// </summary>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        /// <param name="email"></param>
        /// <param name="bedrijf"></param>
        public Bezoeker(string voornaam, string achternaam, string email, string bedrijf) : base(voornaam, achternaam, email) {
			ZetBedrijf(bedrijf);
		}

        /// <summary>
		/// Constructor voor het aanmaken van een bezoeker in de DataLaag.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
        public Bezoeker(uint id, string voornaam, string achternaam, string email, string bedrijf) : base(id, voornaam, achternaam, email)
        {
            ZetBedrijf(bedrijf);
        }

        /// <summary>
        /// Zet bedrijf.
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <exception cref="BezoekerException"></exception>
        public void ZetBedrijf(string bedrijf) {
            if (string.IsNullOrWhiteSpace(bedrijf)) throw new BezoekerException("Bezoeker - ZetBedrijf - bedrijf mag niet leeg zijn");
            Bedrijf = bedrijf;
		}

        /// <summary>
        /// Vergelijkt bezoekers op inhoud.
        /// </summary>
        /// <exception cref="BedrijfException"></exception>
		public bool BezoekerIsGelijk(Bezoeker bezoeker)
		{
            if (bezoeker == null) return false;
            if (bezoeker.Id != Id) return false;
            if (bezoeker.Voornaam != Voornaam) return false;
			if (bezoeker.Achternaam != Achternaam) return false;
			if (bezoeker.Email != Email) return false;
			if (bezoeker.Bedrijf != Bedrijf) return false;
			return true;
		}
	}
}