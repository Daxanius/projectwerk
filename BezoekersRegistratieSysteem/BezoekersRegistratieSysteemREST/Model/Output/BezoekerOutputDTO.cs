using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class BezoekerOutputDTO {
		public static BezoekerOutputDTO NaarDTO(Bezoeker bezoeker) {
			return new(bezoeker.Id, bezoeker.Voornaam, bezoeker.Achternaam, bezoeker.Email, bezoeker.Bedrijf);
		}

		public static IEnumerable<BezoekerOutputDTO> NaarDTO(IEnumerable<Bezoeker> bezoekers) {
			List<BezoekerOutputDTO> output = new();
			foreach (Bezoeker bezoeker in bezoekers) {
				output.Add(BezoekerOutputDTO.NaarDTO(bezoeker));
			}
			return output;
		}

		public BezoekerOutputDTO(uint id, string voornaam, string achternaam, string email, string bedrijf) {
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
