// using BezoekersRegistratieSysteemBL.Domeinen;
// using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
// using BezoekersRegistratieSysteemBL.Interfaces;
//
// namespace BezoekersRegistratieSysteemBL.Managers
// {
//     public class BedrijfsManagerTest
//     {
//         private readonly IBedrijfRepository _bedrijfRepository;
//         private readonly Bezoeker _validBezoeker;
//         private readonly DateTime _validStarttijd;
//         private readonly DateTime _validEindtijd;
//         private readonly Afspraak _validAfspraak;
//         private readonly Werknemer _validWerknemer;
//         private readonly Bedrijf _validBedrijf;
//
//         public BedrijfsManagerTest(IBedrijfRepository bedrijfRepository)
//         {
//             this._bedrijfRepository = bedrijfRepository;
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
//         [Theory]
//         [InlineData()]
//         public void BedrijfsManager_VoegBedrijfToe_Valid(string naam, string btw, string adres, string email,
//             string telefoonnummer)
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public void BedrijfsManager_VerwijderBedrijf_Valid(uint id)
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public Bedrijf BedrijfsManager_GeefBedrijf_Valid(uint id)
//         {
//         }
//
//         [Fact]
//         public void BedrijfsManager_BewerkBedrijf_Valid(Bedrijf bedrijf)
//         {
//         }
//
//         [Fact]
//         public IEnumerable<Bedrijf> BedrijfsManager_Geefbedrijven_Valid()
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public Bedrijf BedrijfsManager_GeefBedrijf_Valid(string bedrijfsnaam)
//         {
//         }
//
//         #endregion
//
//         #region InValid
//
//         [Theory]
//         [InlineData()]
//         public void BedrijfsManager_VoegBedrijfToe_InValid(string naam, string btw, string adres, string email,
//             string telefoonnummer)
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public void BedrijfsManager_VerwijderBedrijf_InValid(uint id)
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public Bedrijf BedrijfsManager_GeefBedrijf_InValid(uint id)
//         {
//         }
//
//         [Fact]
//         public void BedrijfsManager_BewerkBedrijf_InValid(Bedrijf bedrijf)
//         {
//         }
//
//         [Fact]
//         public IEnumerable<Bedrijf> BedrijfsManager_Geefbedrijven_InValid()
//         {
//         }
//
//         [Theory]
//         [InlineData()]
//         public Bedrijf BedrijfsManager_GeefBedrijf_InValid(string bedrijfsnaam)
//         {
//         }
//
//         #endregion
//     }
// }