using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output
{
	public class BedrijfOutputDTO
	{
		public static BedrijfOutputDTO NaarDTO(Bedrijf bedrijf)
		{
			List<long> werknemers = new();
			foreach (Werknemer w in bedrijf.GeefWerknemers())
			{
				werknemers.Add(w.Id);
			}

			return new(bedrijf.Id, bedrijf.Naam, bedrijf.BTW, bedrijf.IsGecontroleert, bedrijf.TelefoonNummer, bedrijf.Email, bedrijf.Adres, werknemers);
		}

		public static IEnumerable<BedrijfOutputDTO> NaarDTO(IEnumerable<Bedrijf> bedrijven)
		{
			List<BedrijfOutputDTO> output = new();
			foreach (Bedrijf bedrijf in bedrijven)
			{
				output.Add(BedrijfOutputDTO.NaarDTO(bedrijf));
			}
			return output;
		}

		public long Id { get; set; }
		public string Naam { get; set; }
		public string BTW { get; set; }
		public bool IsGecontroleert { get; set; }
		public string TelefoonNummer { get; set; }
		public string Email { get; set; }
		public string Adres { get; set; }

		public List<long> Werknemers { get; set; } = new();

		public BedrijfOutputDTO(long id, string naam, string bTW, bool isGecontroleert, string telefoonNummer, string email, string adres, List<long> werknemers)
		{
			Id = id;
			Naam = naam;
			BTW = bTW;
			IsGecontroleert = isGecontroleert;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
			this.Werknemers = werknemers;
		}
	}
}
