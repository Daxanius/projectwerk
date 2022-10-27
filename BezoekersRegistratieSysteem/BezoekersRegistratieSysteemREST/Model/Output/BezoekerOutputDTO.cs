using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class DTOBezoekerOutput {
		public static DTOBezoekerOutput NaarDTO(Bezoeker bezoeker) {
			return new(bezoeker.Id, bezoeker.Voornaam, bezoeker.Achternaam, bezoeker.Email, bezoeker.Bedrijf);
		}

		public static IEnumerable<DTOBezoekerOutput> NaarDTO(IEnumerable<Bezoeker> bezoekers) {
			List<DTOBezoekerOutput> output = new();
			foreach (Bezoeker bezoeker in bezoekers) {
				output.Add(DTOBezoekerOutput.NaarDTO(bezoeker));
			}
			return output;
		}

		public DTOBezoekerOutput(uint id, string voornaam, string achternaam, string email, string bedrijf) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Bedrijf = bedrijf;
		}

		public uint Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public string Email { get; private set; }
		public string Bedrijf { get; private set; }
	}
}
