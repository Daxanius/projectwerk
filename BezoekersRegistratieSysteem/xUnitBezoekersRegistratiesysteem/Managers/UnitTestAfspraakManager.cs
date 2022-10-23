using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace BezoekersRegistratieSysteemBL.Managers
{
	public class AfspraakManagerTest
	{
		//private IAfspraakRepository _afspraakRepository;

		//private readonly Bezoeker validBezoeker;
		//private readonly DateTime validStarttijd;
		//private readonly DateTime validEindtijd;
		//private readonly Afspraak validAfspraak;
		//private readonly Werknemer validWerknemer;
		//private readonly Bedrijf validBedrijf;

		//public AfspraakManagerTest()
		//{
		//	validBezoeker = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
		//	validStarttijd = DateTime.Now;
		//	validEindtijd = DateTime.Now.AddHours(8);
		//	validBedrijf = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
		//	validWerknemer = new(voornaam: "wout", achternaam: "balding", email: "wout@gmail.com");
		//	validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(validBedrijf, "Manager 1");
		//	validAfspraak = new Afspraak(starttijd: validStarttijd, bezoeker: validBezoeker, werknemer: validWerknemer);

		//	_afspraakRepository = new DummyAfspraakRepository();
		//}

		//[Fact]
		//public void MaakAfspraak()
		//{
		//	Afspraak afspraak = new(validStarttijd, validBezoeker, validWerknemer);
		//	afspraak.ZetId(10);

		//	_afspraakRepository.VoegAfspraakToe(afspraak);

		//	Afspraak insertedAfspraak = _afspraakRepository.GeefAfspraak(10);

		//	Assert.Equal(afspraak.Starttijd, insertedAfspraak.Starttijd);
		//	Assert.Equal(afspraak.Id, insertedAfspraak.Id);
		//	Assert.Equal(afspraak.Werknemer, insertedAfspraak.Werknemer);
		//	Assert.Equal(afspraak.Bezoeker, insertedAfspraak.Bezoeker);
		//	Assert.Equal(afspraak.Eindtijd, insertedAfspraak.Eindtijd);
		//}

		//[Fact]
		//public void VerwijderAfspraak()
		//{
		//	Afspraak afspraak = new(validStarttijd, validBezoeker, validWerknemer);
		//	afspraak.ZetId(10);

		//	_afspraakRepository.VoegAfspraakToe(afspraak);

		//	Afspraak insertedAfspraak = _afspraakRepository.GeefAfspraak(10);

		//	Assert.Equal(afspraak.Starttijd, insertedAfspraak.Starttijd);
		//	Assert.Equal(afspraak.Id, insertedAfspraak.Id);
		//	Assert.Equal(afspraak.Werknemer, insertedAfspraak.Werknemer);
		//	Assert.Equal(afspraak.Bezoeker, insertedAfspraak.Bezoeker);
		//	Assert.Equal(afspraak.Eindtijd, insertedAfspraak.Eindtijd);

		//	_afspraakRepository.VerwijderAfspraak(afspraak.Id);

		//	Assert.Throws<AfspraakException>(() => _afspraakRepository.GeefAfspraak(10));
		//}

		//[Fact]
		//public void GeefAfspraakOpId()
		//{
		//	Afspraak validAfspraakRepo = ((DummyAfspraakRepository)_afspraakRepository).Afspraak1;

		//	Afspraak afspraak = _afspraakRepository.GeefAfspraak(validAfspraakRepo.Id);

		//	Assert.Equal(afspraak.Starttijd, validAfspraakRepo.Starttijd);
		//	Assert.Equal(afspraak.Id, validAfspraakRepo.Id);
		//	Assert.Equal(afspraak.Werknemer, validAfspraakRepo.Werknemer);
		//	Assert.Equal(afspraak.Bezoeker, validAfspraakRepo.Bezoeker);
		//	Assert.Equal(afspraak.Eindtijd, validAfspraakRepo.Eindtijd);
		//}

		//[Fact]
		//public void BewerkAfspraak()
		//{
		//	Afspraak validAfspraakRepo = ((DummyAfspraakRepository)_afspraakRepository).Afspraak1;

		//	Afspraak afspraak = validAfspraakRepo;
		//	DateTime startTijd = DateTime.Now;

		//	afspraak.ZetStarttijd(startTijd);

		//	_afspraakRepository.BewerkAfspraak(afspraak);

		//	Afspraak afspraakFromRepo = _afspraakRepository.GeefAfspraak(validAfspraakRepo.Id);

		//	Assert.Equal(afspraak.Starttijd, afspraakFromRepo.Starttijd);
		//	Assert.Equal(afspraak.Id, afspraakFromRepo.Id);
		//	Assert.Equal(afspraak.Werknemer, afspraakFromRepo.Werknemer);
		//	Assert.Equal(afspraak.Bezoeker, afspraakFromRepo.Bezoeker);
		//	Assert.Equal(afspraak.Eindtijd, afspraakFromRepo.Eindtijd);
		//}

		//[Fact]
		//public void BeeindigAfspraak()
		//{
		//	Afspraak validAfspraakRepo = ((DummyAfspraakRepository)_afspraakRepository).Afspraak1;
		//	List<Afspraak> afspraken = _afspraakRepository.GeefHuidigeAfspraken().ToList();

		//	Assert.Contains(validAfspraakRepo, afspraken);

		//	_afspraakRepository.BeeindigAfspraakBezoeker(validAfspraakRepo.Id);
		//	afspraken = _afspraakRepository.GeefHuidigeAfspraken().ToList();

		//	Assert.DoesNotContain(validAfspraakRepo, afspraken);
		//}

		//[Fact]
		//public void GeefHuidigeAfspraken()
		//{
		//	Afspraak afspraak = new(DateTime.Now, validBezoeker, validWerknemer);
		//	afspraak.ZetId(10);

		//	_afspraakRepository.VoegAfspraakToe(afspraak);
		//	List<Afspraak> afspraken = _afspraakRepository.GeefHuidigeAfspraken().ToList();

		//	Assert.Contains(afspraak, afspraken);
		//}

		//[Fact]
		//public void GeefHuidigeAfsprakenPerBedrijf()
		//{
		//	Afspraak afspraak = new(DateTime.Now, validBezoeker, validWerknemer);
		//	afspraak.ZetId(10);

		//	_afspraakRepository.VoegAfspraakToe(afspraak);
		//	List<Afspraak> afspraken = _afspraakRepository.GeefHuidigeAfsprakenPerBedrijf(validBedrijf.Id).ToList();

		//	Assert.Contains(afspraak, afspraken);
		//}

		//[Fact]
		//public void GeefAlleAfsprakenPerWerknemer()
		//{
		//	Werknemer validWerknemerRepo = ((DummyAfspraakRepository)_afspraakRepository).Werknemer1;
				
		//	Afspraak afspraak = new(DateTime.Now, validBezoeker, validWerknemer);
		//	afspraak.ZetId(10);

		//	Afspraak afspraakMetAndereWerknemer = new(DateTime.Now, validBezoeker, validWerknemerRepo);
		//	afspraakMetAndereWerknemer.ZetId(12);

		//	_afspraakRepository.VoegAfspraakToe(afspraak);
		//	_afspraakRepository.VoegAfspraakToe(afspraakMetAndereWerknemer);
			
		//	List<Afspraak> afspraken = _afspraakRepository.GeefAlleAfsprakenPerWerknemer(validWerknemer.Id).ToList();

		//	Assert.Contains(afspraak, afspraken);
		//	Assert.DoesNotContain(afspraakMetAndereWerknemer, afspraken);
		//}

		//[Fact]
		//public void GeefHuidigeAfsprakenPerWerknemer()
		//{
		//	Werknemer validWerknemerRepo = ((DummyAfspraakRepository)_afspraakRepository).Werknemer1;

		//	Afspraak afspraak = new(DateTime.Now, validBezoeker, validWerknemer);
		//	afspraak.ZetId(10);

		//	Afspraak afspraakMetAndereWerknemer = new(DateTime.Now, validBezoeker, validWerknemerRepo);
		//	afspraakMetAndereWerknemer.ZetId(12);

		//	_afspraakRepository.VoegAfspraakToe(afspraak);
		//	_afspraakRepository.VoegAfspraakToe(afspraakMetAndereWerknemer);

		//	List<Afspraak> afspraken = _afspraakRepository.GeefAlleAfsprakenPerWerknemer(validWerknemer.Id).ToList();

		//	Assert.Contains(afspraak, afspraken);
		//	Assert.DoesNotContain(afspraakMetAndereWerknemer, afspraken);
		//}

		//[Fact]
		//public void GeefAfsprakenPerWerknemerOpDag()
		//{
		//	Werknemer validWerknemerRepo = ((DummyAfspraakRepository)_afspraakRepository).Werknemer1;

		//	Afspraak afspraak = new(DateTime.Now, validBezoeker, validWerknemer);
		//	afspraak.ZetId(10);

		//	Afspraak afspraakMetAndereDag = new(DateTime.Now.AddDays(2), validBezoeker, validWerknemer);
		//	afspraakMetAndereDag.ZetId(12);

		//	Afspraak afspraakMetAndereWerknemer = new(DateTime.Now, validBezoeker, validWerknemerRepo);
		//	afspraakMetAndereWerknemer.ZetId(14);

		//	_afspraakRepository.VoegAfspraakToe(afspraak);
		//	_afspraakRepository.VoegAfspraakToe(afspraakMetAndereDag);
		//	_afspraakRepository.VoegAfspraakToe(afspraakMetAndereWerknemer);

		//	List<Afspraak> afspraken = _afspraakRepository.GeefAfsprakenPerWerknemerOpDag(validWerknemer.Id, DateTime.Now).ToList();

		//	Assert.Contains(afspraak, afspraken);
		//	Assert.DoesNotContain(afspraakMetAndereWerknemer, afspraken);
		//	Assert.DoesNotContain(afspraakMetAndereDag, afspraken);
		//}

		//[Fact]
		//public void GeefAfsprakenPerDag()
		//{
		//	Afspraak afspraak = new(DateTime.Now, validBezoeker, validWerknemer);
		//	afspraak.ZetId(10);

		//	Afspraak afspraakMetAndereDag = new(DateTime.Now.AddDays(2), validBezoeker, validWerknemer);
		//	afspraakMetAndereDag.ZetId(12);

		//	_afspraakRepository.VoegAfspraakToe(afspraak);
		//	_afspraakRepository.VoegAfspraakToe(afspraakMetAndereDag);

		//	List<Afspraak> afspraken = _afspraakRepository.GeefAfsprakenPerDag(DateTime.Now).ToList();

		//	Assert.Contains(afspraak, afspraken);
		//	Assert.DoesNotContain(afspraakMetAndereDag, afspraken);
		//}
	}
}