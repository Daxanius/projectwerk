﻿using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output
{
	/// <summary>
	/// De DTO voor uitgaande bedrijf informatie.
	/// </summary>
	public class BedrijfOutputDTO
	{
		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <returns>De DTO variant.</returns>
		public static BedrijfOutputDTO NaarDTO(Bedrijf bedrijf)
		{
			List<long> werknemers = new();
			foreach (Werknemer w in bedrijf.GeefWerknemers())
			{
				werknemers.Add(w.Id);
			}

			return new(bedrijf.Id, bedrijf.Naam, bedrijf.BTW, bedrijf.BtwGeverifieerd, bedrijf.TelefoonNummer, bedrijf.Email, bedrijf.Adres, werknemers);
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="bedrijven"></param>
		/// <returns>Een lijst van de DTO variant.</returns>
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
		public bool IsGecontroleerd { get; set; }
		public string TelefoonNummer { get; set; }
		public string Email { get; set; }
		public string Adres { get; set; }

		public List<long> Werknemers { get; set; } = new();

		/// <summary>
		/// De constructor
		/// </summary>
		/// <param name="id"></param>
		/// <param name="naam"></param>
		/// <param name="bTW"></param>
		/// <param name="isGecontroleert"></param>
		/// <param name="telefoonNummer"></param>
		/// <param name="email"></param>
		/// <param name="adres"></param>
		/// <param name="werknemers"></param>
		public BedrijfOutputDTO(long id, string naam, string bTW, bool isGecontroleert, string telefoonNummer, string email, string adres, List<long> werknemers)
		{
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
