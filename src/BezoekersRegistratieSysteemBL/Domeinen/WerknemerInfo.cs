﻿using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {

	public class WerknemerInfo : Status {
		public Bedrijf Bedrijf { get; private set; }
		public string Email { get; private set; }

		private List<string> Functies = new List<string>();



		/// <summary>
		/// Constructor voor het aanmaken van werknemerInfo.
		/// </summary>
		public WerknemerInfo(Bedrijf bedrijf, string email, string statusNaam) : this(bedrijf, email) {
			base.ZetStatusNaam(statusNaam);
		}

		/// <summary>
		/// Constructor voor het aanmaken van werknemerInfo.
		/// </summary>
		public WerknemerInfo(Bedrijf bedrijf, string email) {
			ZetBedrijf(bedrijf);
			ZetEmail(email);
		}
		public WerknemerInfo() {
		}
		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt bedrijf in.
		/// </summary>
		/// <param name="bedrijf">Mag geen Null waarde zijn.</param>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - ZetBedrijf - bedrijf mag niet leeg zijn"</exception>
		public void ZetBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new WerknemerInfoException("WerknemerInfo - ZetBedrijf - bedrijf mag niet leeg zijn");
			Bedrijf = bedrijf;
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt het mailadres in.
		/// </summary>
		/// <param name="email">Mag geen Null/WhiteSpace waarde zijn en moet aan de voorwaarden voldoen.</param>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - ZetEmail - email mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - ZetEmail - email is niet geldig"</exception>
		public void ZetEmail(string email) {
			if (string.IsNullOrWhiteSpace(email))
				throw new WerknemerInfoException("WerknemerInfo - ZetEmail - email mag niet leeg zijn");
			//Checkt of email geldig is
			if (Nutsvoorziening.IsEmailGeldig(email.Trim()))
				Email = email.Trim();
			else
				throw new WerknemerInfoException("WerknemerInfo - ZetEmail - email is niet geldig");
		}

		/// <summary>
		/// Haalt een lijst functies op met enkel lees rechten voor een werknemer.
		/// </summary>
		/// <returns>IReadOnlyList van strings (functies).</returns>
		public IReadOnlyList<string> GeefWerknemerFuncties() {
			return Functies.AsReadOnly();
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en voegt functie toe aan werknemer.
		/// </summary>
		/// <param name="functie">Mag geen Null/WhiteSpace waarde zijn.</param>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - VoegWerknemerFunctieToe - functie mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - VoegWerknemerFunctieToe - werknemer heeft deze functie al"</exception>
		public void VoegWerknemerFunctieToe(string functie) {
			if (string.IsNullOrWhiteSpace(functie))
				throw new WerknemerInfoException("WerknemerInfo - VoegWerknemerFunctieToe - functie mag niet leeg zijn");
			functie = Nutsvoorziening.NaamOpmaak(functie);
			if (Functies.Contains(functie))
				throw new WerknemerInfoException("WerknemerInfo - VoegWerknemerFunctieToe - werknemer heeft deze functie al");
			Functies.Add(functie);
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en wijzigt functie van werknemer.
		/// </summary>
		/// <param name="oudefunctie">Mag geen Null/WhiteSpace waarde zijn.</param>
		/// <param name="nieuwefunctie">Mag geen Null/WhiteSpace waarde zijn.</param>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - VerwijderWerknemerFunctie - oude functie mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - VerwijderWerknemerFunctie - nieuwe functie mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - VerwijderWerknemerFunctie - werknemer heeft deze functie niet"</exception>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - VerwijderWerknemerFunctie - werknemer heeft deze functie al"</exception>
		public void WijzigWerknemerFunctie(string oudefunctie, string nieuwefunctie) {
			if (string.IsNullOrWhiteSpace(oudefunctie))
				throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - oude functie mag niet leeg zijn");
			if (string.IsNullOrWhiteSpace(nieuwefunctie))
				throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - nieuwe functie mag niet leeg zijn");
			oudefunctie = Nutsvoorziening.NaamOpmaak(oudefunctie);
			nieuwefunctie = Nutsvoorziening.NaamOpmaak(nieuwefunctie);
			if (!Functies.Contains(oudefunctie))
				throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - werknemer heeft deze functie niet");
			if (Functies.Contains(nieuwefunctie))
				throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - werknemer heeft deze functie al");
			Functies.Remove(oudefunctie);
			Functies.Add(nieuwefunctie);
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en verwijdert functie van werknemer.
		/// </summary>
		/// <param name="functie">Mag geen Null/WhiteSpace waarde zijn.</param>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - VerwijderWerknemerFunctie - functie mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - VerwijderWerknemerFunctie - werknemer heeft deze functie niet"</exception>
		/// <exception cref="WerknemerInfoException">"WerknemerInfo - VerwijderWerknemerFunctie - werknemer moet minstens 1 functie hebben"</exception>
		public void VerwijderWerknemerFunctie(string functie) {
			if (string.IsNullOrWhiteSpace(functie))
				throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - functie mag niet leeg zijn");
			functie = Nutsvoorziening.NaamOpmaak(functie);
			if (!Functies.Contains(functie))
				throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - werknemer heeft deze functie niet");
			if (Functies.Count == 1)
				throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - werknemer moet minstens 1 functie hebben");

			Functies.Remove(functie);
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en properties op gelijkheid.
		/// </summary>
		/// <param name="werknemerinfo">Te vergelijken werknemerInfo.</param>
		/// <returns>Boolean True als alle waarden gelijk zijn | False indien één of meerdere waarde(n) verschillend zijn.</returns>
		public bool WerknemerInfoIsGelijk(WerknemerInfo werknemerinfo) {
			if (werknemerinfo is null)
				return false;
			if (werknemerinfo.Bedrijf != Bedrijf)
				return false;
			if (werknemerinfo.Email != Email)
				return false;
			foreach (string functie in Functies) {
				if (!werknemerinfo.Functies.Contains(functie))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Zet statusnaam voor werknemerInfo, dus werknemer per bedrijf
		/// </summary>
		/// <param name="statusNaam">Status naam die gezet moet worden.</param>
		public void ZetStatusNaam(string statusNaam) {
			if (String.IsNullOrWhiteSpace(statusNaam)) {
				throw new WerknemerInfoException("Status naam is leeg");
			}
			base.ZetStatusNaam(statusNaam);
		}
	}
}