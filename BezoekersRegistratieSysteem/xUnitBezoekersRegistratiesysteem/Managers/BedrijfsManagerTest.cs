//using BezoekersRegistratieSysteemBL.Domeinen;
//using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
//using BezoekersRegistratieSysteemBL.Interfaces;
//using xUnitBezoekersRegistratiesysteem.DummyData.Repos;

//namespace BezoekersRegistratieSysteemBL.Managers
//{
//	public class BedrijfsManagerTest
//	{
//		private readonly IBedrijfRepository _bedrijfRepository;

//		private readonly Bezoeker validBezoeker;
//		private readonly DateTime validStarttijd;
//		private readonly DateTime validEindtijd;
//		private readonly Afspraak validAfspraak;
//		private readonly Werknemer validWerknemer;
//		private readonly Bedrijf validBedrijf;

//		public BedrijfsManagerTest(IBedrijfRepository bedrijfRepository)
//		{
//			this._bedrijfRepository = bedrijfRepository;

//			validBezoeker = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
//			validStarttijd = DateTime.Now;
//			validEindtijd = DateTime.Now.AddHours(8);
//			validBedrijf = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
//			validWerknemer = new(voornaam: "wout", achternaam: "balding", email: "wout@gmail.com", bedrijf: validBedrijf, functie: "CEO");
//			validAfspraak = new Afspraak(starttijd: validStarttijd, bezoeker: validBezoeker, werknemer: validWerknemer);

//			_bedrijfRepository = new DummyBedrijfsRepository();
//		}

//		public void VoegBedrijfToe()
//		{
//			List<Bedrijf> bedrijven = _bedrijfRepository.GeefBedrijven().ToList();

//			Assert.DoesNotContain(validBedrijf, bedrijven);

//			_bedrijfRepository.VoegBedrijfToe(validBedrijf);

//			bedrijven = _bedrijfRepository.GeefBedrijven().ToList();

//			Assert.Contains(validBedrijf, bedrijven);
//		}

//		public void VerwijderBedrijfById()
//		{
//			_bedrijfRepository.VoegBedrijfToe(validBedrijf);

//			List<Bedrijf> bedrijven = _bedrijfRepository.GeefBedrijven().ToList();

//			Assert.Contains(validBedrijf, bedrijven);

//			_bedrijfRepository.VerwijderBedrijf(validBedrijf.Id);

//			bedrijven = _bedrijfRepository.GeefBedrijven().ToList();

//			Assert.DoesNotContain(validBedrijf, bedrijven);
//		}

//		public void GeefBedrijfById()
//		{
//			_bedrijfRepository.VoegBedrijfToe(validBedrijf);

//			List<Bedrijf> bedrijven = _bedrijfRepository.GeefBedrijven().ToList();

//			Assert.Contains(validBedrijf, bedrijven);

//			Bedrijf insertedBedrijf = _bedrijfRepository.GeefBedrijfById(validBedrijf.Id);

//			Assert.Equal(validBedrijf, insertedBedrijf);
//		}

//		public void WijzigBedrijf()
//		{
			
//		}

//		public IEnumerable<Bedrijf> Geefbedrijven()
//		{

//		}

//		public Bedrijf GeefBedrijfOpNaam()
//		{

//		}
//	}
//}