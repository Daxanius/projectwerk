// using BezoekersRegistratieSysteemBL.Domeinen;
// using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
// using BezoekersRegistratieSysteemBL.Interfaces;
//
// namespace BezoekersRegistratieSysteemBL.Managers
// {
//     public class AfspraakManagerTest
//     {
//         private readonly IAfspraakRepository _afspraakRepository;
//         private readonly Bezoeker _validBezoeker;
//         private readonly DateTime _validStarttijd;
//         private readonly DateTime _validEindtijd;
//         private readonly Afspraak _validAfspraak;
//         private readonly Werknemer _validWerknemer;
//         private readonly Bedrijf _validBedrijf;
//
//         public AfspraakManagerTest(IAfspraakRepository afspraakRepository)
//         {
//             _afspraakRepository = afspraakRepository;
//
//             _validBezoeker = new Bezoeker(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com",
//                 bedrijf: "Artevelde");
//             _validStarttijd = DateTime.Now;
//             _validEindtijd = DateTime.Now.AddHours(8);
//             _validBedrijf = new Bedrijf(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242",
//                 email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
//             _validWerknemer = new Werknemer(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com",
//                 bedrijf: _validBedrijf, functie: "CEO");
//             _validAfspraak = new Afspraak(starttijd: _validStarttijd, bezoeker: _validBezoeker,
//                 werknemer: _validWerknemer);
//         }
//
//         #region Valid
//
//         [Fact]
//         public void AfspraakManager_MaakAfspraak_Valid()
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public void AfspraakManager_VerwijderAfspraak_Valid(uint id)
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public Afspraak AfspraakManager_GeefAfspraak_Valid(uint id)
//         {
//         }
//
//         [Fact]
//         public void AfspraakManager_BewerkAfspraak_Valid()
//         {
//         }
//
//         [Fact]
//         public void AfspraakManager_BeeindigAfspraak_Valid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefHuidigeAfspraken_Valid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefHuidigeAfsprakenPerBedrijf_Valid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefAlleAfsprakenPerWerknemer_Valid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefHuidigeAfsprakenPerWerknemer_Valid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefAfsprakenPerWerknemerOpDatum_Valid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefAfsprakenPerDag_Valid()
//         {
//         }
//
//         #endregion
//
//         #region InValid
//
//         [Fact]
//         public void AfspraakManager_MaakAfspraak_InValid()
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public void AfspraakManager_VerwijderAfspraak_InValid(uint id)
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public Afspraak AfspraakManager_GeefAfspraak_InValid(uint id)
//         {
//         }
//
//         [Fact]
//         public void AfspraakManager_BewerkAfspraak_InValid()
//         {
//         }
//
//         [Fact]
//         public void AfspraakManager_BeeindigAfspraak_InValid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefHuidigeAfspraken_InValid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefHuidigeAfsprakenPerBedrijf_InValid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefAlleAfsprakenPerWerknemer_InValid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefHuidigeAfsprakenPerWerknemer_InValid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefAfsprakenPerWerknemerOpDatum_InValid()
//         {
//         }
//
//         [Fact]
//         public IReadOnlyList<Afspraak> AfspraakManager_GeefAfsprakenPerDag_InValid()
//         {
//         }
//
//         #endregion
//     }
// }