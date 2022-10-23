using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers
{
	public class UnitTestBedrijfsManagerTest
	{
		//private readonly IBedrijfRepository _bedrijfRepository;

		//private readonly Bezoeker validBezoeker;
		//private readonly DateTime validStarttijd;
		//private readonly DateTime validEindtijd;
		//private readonly Afspraak validAfspraak;
		//private readonly Werknemer validWerknemer;
		//private readonly Bedrijf validBedrijf;

		//public UnitTestBedrijfsManagerTest()
		//{
		//	validBezoeker = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
		//	validStarttijd = DateTime.Now;
		//	validEindtijd = DateTime.Now.AddHours(8);
		//	validBedrijf = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
		//	validWerknemer = new(voornaam: "wout", achternaam: "balding", email: "wout@gmail.com");
		//	validAfspraak = new Afspraak(starttijd: validStarttijd, bezoeker: validBezoeker, werknemer: validWerknemer);

		//	_bedrijfRepository = new DummyBedrijfsRepository();

		//	validBedrijf.ZetId(1);
		//	validWerknemer.ZetId(2);
		//	validAfspraak.ZetId(3);
		//	validBezoeker.ZetId(4);

		//	validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(validBedrijf, "CEO");
		//}

		//[Fact]
		//public void VoegBedrijfToe()
		//{
		//	_bedrijfRepository.VerwijderBedrijf(validBedrijf.Id);

		//	List<Bedrijf> bedrijven = _bedrijfRepository.Geefbedrijven().ToList();

		//	Assert.DoesNotContain(validBedrijf, bedrijven);

		//	_bedrijfRepository.VoegBedrijfToe(validBedrijf);

		//	bedrijven = _bedrijfRepository.Geefbedrijven().ToList();

		//	Assert.Contains(validBedrijf, bedrijven);
		//}

		//[Fact]
		//public void VerwijderBedrijfById()
		//{
		//	List<Bedrijf> bedrijven = _bedrijfRepository.Geefbedrijven().ToList();

		//	Assert.Contains(validBedrijf, bedrijven);

		//	_bedrijfRepository.VerwijderBedrijf(validBedrijf.Id);

		//	bedrijven = _bedrijfRepository.Geefbedrijven().ToList();

		//	Assert.DoesNotContain(validBedrijf, bedrijven);
		//}

		//[Fact]
		//public void GeefBedrijfById()
		//{
		//	List<Bedrijf> bedrijven = _bedrijfRepository.Geefbedrijven().ToList();

		//	Assert.Contains(validBedrijf, bedrijven);

		//	Bedrijf insertedBedrijf = _bedrijfRepository.GeefBedrijf(validBedrijf.Id);

		//	Assert.Equal(validBedrijf, insertedBedrijf);
		//}

		//[Fact]
		//public void WijzigBedrijf()
		//{
		//	Bedrijf bedrijf = validBedrijf;
		//	bedrijf.ZetAdres("Nieuw Addres");
		//	bedrijf.ZetEmail("email@gmail.com");
		//	bedrijf.ZetId(4);
		//	bedrijf.ZetTelefoonNummer("04766927844");

		//	_bedrijfRepository.VoegBedrijfToe(bedrijf);
		//	_bedrijfRepository.BewerkBedrijf(bedrijf);

		//	Bedrijf insertedBedrijf = _bedrijfRepository.GeefBedrijf(bedrijf.Id);

		//	Assert.Equal(bedrijf, insertedBedrijf);
		//}

		//[Fact]
		//public void Geefbedrijven()
		//{
		//	List<Bedrijf> bedrijven = _bedrijfRepository.Geefbedrijven().ToList();
		//	Assert.Contains(validBedrijf, bedrijven);
		//}

		//[Theory]
		//[InlineData("aNaam")]
		//[InlineData("bNaam")]
		//[InlineData("cNaam")]
		//public void GeefBedrijfOpNaam(string bedrijfsNaam)
		//{
		//	Bedrijf bedrijf = _bedrijfRepository.GeefBedrijf(bedrijfsNaam);
		//	Assert.Equal(bedrijfsNaam, bedrijf.Naam);
		//}

		//[Theory]
		//[InlineData(1)]
		//[InlineData(2)]
		//[InlineData(3)]
		//public void GeefBedrijfOpId(uint bedrijfsId)
		//{
		//	Bedrijf bedrijf = _bedrijfRepository.GeefBedrijf(bedrijfsId);
		//	Assert.Equal(bedrijfsId, bedrijf.Id);
		//}
	}
}