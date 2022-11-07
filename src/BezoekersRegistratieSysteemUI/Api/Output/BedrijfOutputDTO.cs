using System.Collections.Generic;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	/// <summary>
	/// De DTO voor uitgaande bedrijf informatie.
	/// </summary>
	public class BedrijfOutputDTO {
		/// <summary>
		/// De ID van het bedrijf.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// De naam van het bedrijf.
		/// </summary>
		public string Naam { get; set; }

		/// <summary>
		/// Het BTW nummer van het bedrijf.
		/// </summary>
		public string BTW { get; set; }

		/// <summary>
		/// Zegt of het BTW nummer gecontroleert is op bestaan.
		/// </summary>
		public bool IsGecontroleerd { get; set; }

		/// <summary>
		/// Het telefoon nummer van het bedrijf.
		/// </summary>
		public string TelefoonNummer { get; set; }

		/// <summary>
		/// Het mailadres van het bedrijf.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Het adres van het bedrijf.
		/// </summary>
		public string Adres { get; set; }

		/// <summary>
		/// De werknemers van het bedrijf.
		/// </summary>
		public List<IdInfoOutputDTO> Werknemers { get; set; } = new();

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="naam"></param>
		/// <param name="bTW"></param>
		/// <param name="isGecontroleert"></param>
		/// <param name="telefoonNummer"></param>
		/// <param name="email"></param>
		/// <param name="adres"></param>
		/// <param name="werknemers"></param>
		public BedrijfOutputDTO(long id, string naam, string bTW, bool isGecontroleert, string telefoonNummer, string email, string adres, List<IdInfoOutputDTO> werknemers) {
			Id = id;
			Naam = naam;
			BTW = bTW;
			IsGecontroleerd = isGecontroleert;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
			this.Werknemers = werknemers;
		}
	}
}
