// using BezoekersRegistratieSysteemBL.Domeinen;
// using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
// using BezoekersRegistratieSysteemBL.Interfaces;
//
// namespace BezoekersRegistratieSysteemBL.Managers {
// 	public class WerknemerManagerTest {
// 		private readonly IWerknemerRepository _werknemerRepository;
// 		private readonly Bezoeker _validBezoeker;
// 		private readonly DateTime _validStarttijd;
// 		private readonly DateTime _validEindtijd;
// 		private readonly Afspraak _validAfspraak;
// 		private readonly Werknemer _validWerknemer;
// 		private readonly Bedrijf _validBedrijf;
//
// 		public WerknemerManagerTest(IWerknemerRepository werknemerRepository) {
// 			this._werknemerRepository = werknemerRepository;
// 			
// 			_validBezoeker = new Bezoeker(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com",
// 				bedrijf: "Artevelde");
// 			_validStarttijd = DateTime.Now;
// 			_validEindtijd = DateTime.Now.AddHours(8);
// 			_validBedrijf = new Bedrijf(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242",
// 				email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
// 			_validWerknemer = new Werknemer(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com",
// 				bedrijf: _validBedrijf, functie: "CEO");
// 			_validAfspraak = new Afspraak(starttijd: _validStarttijd, bezoeker: _validBezoeker,
// 				werknemer: _validWerknemer);
// 		}
//
// 		#region Valid
// 		
// 		[Theory]
// 		[InlineData()]
// 		public void WerknemerManagerTest_VoegWerknemerToe_Valid(string voornaam, string achternaam, string email, string functie) {
// 			
// 		}
//
// 		[Theory]
// 		[InlineData()]
// 		public void WerknemerManagerTest_VerwijderWerknemer_Valid(uint id) {
// 			
// 		}
//
// 		[Theory]
// 		[InlineData()]
// 		public Werknemer WerknemerManagerTest_GeefWerknemer_Valid(uint id) {
// 			
// 		}
//
// 		[Fact]
// 		public void WerknemerManagerTest_WijzigWerknemer_Valid() {
// 			
// 		}
//
// 		[Theory]
// 		[InlineData()]
// 		public Werknemer WerknemerManagerTest_GeefWerknemerOpNaam_Valid(string naam) {
// 			
// 		}
//
// 		[Fact]
// 		public IReadOnlyList<Werknemer> WerknemerManagerTest_GeefWerknemersPerBedrijf_Valid() {
// 			
// 		}
//
// 		[Theory]
// 		[InlineData()]
// 		public IReadOnlyList<Werknemer> WerknemerManagerTest_GeefWerknemersPerFunctie_Valid(string functie) {
// 			
// 		}
//
// 		#endregion
// 		
// 		#region InValid
// 		
// 		[Theory]
// 		[InlineData()]
// 		public void WerknemerManagerTest_VoegWerknemerToe_InValid(string voornaam, string achternaam, string email, string functie) {
// 			
// 		}
//
// 		[Theory]
// 		[InlineData()]
// 		public void WerknemerManagerTest_VerwijderWerknemer_InValid(uint id) {
// 			
// 		}
//
// 		[Theory]
// 		[InlineData()]
// 		public Werknemer WerknemerManagerTest_GeefWerknemer_InValid(uint id) {
// 			
// 		}
//
// 		[Fact]
// 		public void WerknemerManagerTest_WijzigWerknemer_InValid() {
// 			
// 		}
//
// 		[Theory]
// 		[InlineData()]
// 		public Werknemer WerknemerManagerTest_GeefWerknemerOpNaam_InValid(string naam) {
// 			
// 		}
//
// 		[Fact]
// 		public IReadOnlyList<Werknemer> WerknemerManagerTest_GeefWerknemersPerBedrijf_InValid() {
// 			
// 		}
//
// 		[Theory]
// 		[InlineData()]
// 		public IReadOnlyList<Werknemer> WerknemerManagerTest_GeefWerknemersPerFunctie_InValid(string functie) {
// 			
// 		}
//
// 		#endregion
// 	}
// }