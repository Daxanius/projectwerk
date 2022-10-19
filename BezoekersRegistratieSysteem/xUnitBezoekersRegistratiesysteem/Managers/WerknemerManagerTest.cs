using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using xUnitBezoekersRegistratiesysteem.DummyData.Repos;

namespace BezoekersRegistratieSysteemBL.Managers
{
	public class WerknemerManagerTest
	{
		private readonly IWerknemerRepository _werknemerRepository;
		private readonly Bezoeker _validBezoeker;
		private readonly DateTime _validStarttijd;
		private readonly DateTime _validEindtijd;
		private readonly Afspraak _validAfspraak;
		private readonly Werknemer _validWerknemer;
		private readonly Bedrijf _validBedrijf;

		public WerknemerManagerTest()
		{
			_werknemerRepository = new DummyWerknemerRepository();

			_validBezoeker = new Bezoeker(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
			_validStarttijd = DateTime.Now;
			_validEindtijd = DateTime.Now.AddHours(8);
			_validBedrijf = new Bedrijf(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
			_validWerknemer = new Werknemer(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com");
			_validAfspraak = new Afspraak(starttijd: _validStarttijd, bezoeker: _validBezoeker, werknemer: _validWerknemer);

			_validBedrijf.ZetId(1);
			_validWerknemer.ZetId(2);
			_validAfspraak.ZetId(3);
			_validBezoeker.ZetId(4);

			_validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(_validBedrijf, "CEO");
		}

		[Fact]
		public void WerknemerManagerTest_VoegWerknemerToe()
		{
			Werknemer insertedWerknemer = _werknemerRepository.VoegWerknemerToe(_validWerknemer);

			Assert.True(_validWerknemer.WerknemerIsGelijk(insertedWerknemer));
		}

		[Fact]
		public void WerknemerManagerTest_VerwijderWerknemer()
		{
			Werknemer insertedWerknemer = _werknemerRepository.VoegWerknemerToe(_validWerknemer);

			_werknemerRepository.GeefWerknemer(_validWerknemer.Id);

			Assert.True(_validWerknemer.WerknemerIsGelijk(insertedWerknemer));

			_werknemerRepository.VerwijderWerknemer(_validWerknemer.Id);

			Assert.Throws<WerknemerException>(() => _werknemerRepository.VerwijderWerknemer(_validWerknemer.Id));
		}

		[Fact]
		public void WerknemerManagerTest_GeefWerknemer()
		{
			_werknemerRepository.VoegWerknemerToe(_validWerknemer);

			Werknemer werknemer = _werknemerRepository.GeefWerknemer(_validWerknemer.Id);

			Assert.True(_validWerknemer.WerknemerIsGelijk(werknemer));
		}

		[Fact]
		public void WerknemerManagerTest_WijzigWerknemer()
		{
			Werknemer Insertedwerknemer = _werknemerRepository.VoegWerknemerToe(_validWerknemer);

			Werknemer werknemer = _werknemerRepository.GeefWerknemer(Insertedwerknemer.Id);
			werknemer.ZetEmail("balding@gmail.com");

			_werknemerRepository.WijzigWerknemer(werknemer);

			Werknemer gewijzigdeWerknemer = _werknemerRepository.GeefWerknemer(Insertedwerknemer.Id);

			Assert.Equal(gewijzigdeWerknemer.Email, werknemer.Email);
		}

		[Fact]
		public void WerknemerManagerTest_GeefWerknemerOpNaam()
		{
			string voorNaam = _validWerknemer.Voornaam;
			string achterNaam = _validWerknemer.Achternaam;

			_werknemerRepository.VoegWerknemerToe(_validWerknemer);

			var werknemersMetNaam = _werknemerRepository.GeefWerknemersOpNaam(voorNaam, achterNaam);

			Assert.True(werknemersMetNaam.All(w => w.Voornaam.ToLower() == voorNaam.ToLower() && w.Achternaam.ToLower() == achterNaam.ToLower()));
		}

		[Fact]
		public void WerknemerManagerTest_GeefWerknemersPerBedrijf()
		{
			_werknemerRepository.VoegWerknemerToe(_validWerknemer);

			var werknemersPerBedrijf = _werknemerRepository.GeefWerknemersPerBedrijf(_validBedrijf.Id);

			var bedrijven = _validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Select(b => b.Key).Where(b => b.BedrijfIsGelijk(_validBedrijf));

			Assert.Contains(_validBedrijf, bedrijven);
		}

		[Fact]
		public void WerknemerManagerTest_GeefWerknemersPerFunctie()
		{
			_werknemerRepository.VoegWerknemerToe(_validWerknemer);

			var functies = _validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Select(b => b.Value).Where(f => f.Contains("CEO")).First();

			Assert.Contains("CEO", functies);
		}
	}
}