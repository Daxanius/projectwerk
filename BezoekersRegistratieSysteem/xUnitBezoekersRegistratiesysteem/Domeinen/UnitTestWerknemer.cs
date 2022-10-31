using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domeinen
{
	public class UnitTestWerknemer
	{
		//AF

		#region Valid Info
		private Bedrijf _b1 = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
		private Bedrijf _b2 = new(1, "anderbedrijf", "BE0724540609", true, "876543210", "anderbedrijf@email.com", "anderebedrijfstraat 10");
		private string _of = "oudefunctie";
		private string _nf = "nieuwefunctie";
		private string _e = "werknemer.werknemersen@email.com";
		#endregion

		#region UnitTest Id
		[Fact]
		public void ZetId_Valid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			w.ZetId(10);
			Assert.Equal((long)10, w.Id);
		}

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ZetId_Invalid(long id)
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			Assert.Throws<WerknemerException>(() => w.ZetId(id));
		}
		#endregion

		#region UnitTest Voornaam
		[Fact]
		public void ZetVoornaam_Valid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			w.ZetVoornaam("werknemer");
			Assert.Equal("werknemer", w.Voornaam);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void ZetVoornaam_Invalid(string voornaam)
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			Assert.Throws<WerknemerException>(() => w.ZetVoornaam(voornaam));
		}
		#endregion

		#region UnitTest Achternaam
		[Fact]
		public void ZetAchternaam_Valid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			w.ZetAchternaam("werknemersen");
			Assert.Equal("werknemersen", w.Achternaam);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void ZetAchternaam_Invalid(string achternaam)
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			Assert.Throws<WerknemerException>(() => w.ZetAchternaam(achternaam));
		}
		#endregion

		#region UnitTest Voeg Bedrijf & Functie Toe Aan Werknemer
		[Fact]
		public void VoegBedrijvenEnFunctieToeAanWerknemer_Valid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _nf);
			IReadOnlyDictionary<Bedrijf, WerknemerInfo> actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
			Assert.Collection(actual,
				expected =>
				{
					Assert.Equal(_b1, expected.Key);
					Assert.Equal(_e, expected.Value.Email);
					Assert.Collection(expected.Value.GeefWerknemerFuncties(),
						item => Assert.Equal(_nf, item));
				});

			//meerdere functies check
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _of);
			actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
			Assert.Collection(actual,
				expected =>
				{
					Assert.Equal(_b1, expected.Key);
					Assert.Equal(_e, expected.Value.Email);
					Assert.Collection(expected.Value.GeefWerknemerFuncties(),
						item => Assert.Equal(_nf, item),
						item => Assert.Equal(_of, item));
				});
		}

		[Theory]
		[InlineData(null, "nieuwefunctie")]
		[InlineData("", "nieuwefunctie")]
		[InlineData(" ", "nieuwefunctie")]
		[InlineData("\n", "nieuwefunctie")]
		[InlineData("\r", "nieuwefunctie")]
		[InlineData("\t", "nieuwefunctie")]
		[InlineData("\v", "nieuwefunctie")]
		[InlineData("@email.com", "nieuwefunctie")]
		[InlineData("werknemer.werknemersen@email.", "nieuwefunctie")]
		[InlineData("werknemer.werknemersen@.com", "nieuwefunctie")]
		[InlineData("werknemer.werknemersen@email", "nieuwefunctie")]
		[InlineData("werknemer.werknemersen@", "nieuwefunctie")]
		[InlineData("werknemer.werknemersen", "nieuwefunctie")]
		[InlineData("werknemer.werknemersen@.", "nieuwefunctie")]
		[InlineData("werknemer.werknemersen.com", "nieuwefunctie")]
		[InlineData("werknemer.werknemersen@email.com", "nieuwefunctie")]
		[InlineData("werknemer.werknemersen@email.com", null)]
		[InlineData("werknemer.werknemersen@email.com", "")]
		[InlineData("werknemer.werknemersen@email.com", " ")]
		[InlineData("werknemer.werknemersen@email.com", "\n")]
		[InlineData("werknemer.werknemersen@email.com", "\r")]
		[InlineData("werknemer.werknemersen@email.com", "\t")]
		[InlineData("werknemer.werknemersen@email.com", "\v")]
		public void VoegBedrijvenEnFunctieToeAanWerknemer_Invalid(string email, string functie)
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(null, email, functie));
			//CHECK duplicates
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _nf);
			Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _nf));
		}
		#endregion

		#region UnitTest Verwijder Bedrijf
		[Fact]
		public void VerwijderBedrijfVanWerknemer_Valid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _nf);
			w.VerwijderBedrijfVanWerknemer(_b1);
			IReadOnlyDictionary<Bedrijf, WerknemerInfo> actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
			Assert.Empty(actual);

			//Meerdere bedrijven check
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _nf);
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b2, _e, _nf);
			w.VerwijderBedrijfVanWerknemer(_b1);
			actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
			Assert.Collection(actual,
				expected =>
				{
					Assert.Equal(_b2, expected.Key);
					Assert.Equal(_e, expected.Value.Email);
					Assert.Collection(expected.Value.GeefWerknemerFuncties(),
						item => Assert.Equal(_nf, item));
				});
		}

		[Fact]
		public void VerwijderBedrijfVanWerknemer_Invalid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			Assert.Throws<WerknemerException>(() => w.VerwijderBedrijfVanWerknemer(null));
			Assert.Throws<WerknemerException>(() => w.VerwijderBedrijfVanWerknemer(_b1));
		}
		#endregion

		#region UnitTest Wijzig Functie
		[Fact]
		public void WijzigFunctie_Valid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _of);
			w.WijzigFunctie(_b1, _of, _nf);
			IReadOnlyDictionary<Bedrijf, WerknemerInfo> actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
			Assert.Collection(actual,
				expected =>
				{
					Assert.Equal(_b1, expected.Key);
					Assert.Equal(_e, expected.Value.Email);
					Assert.Collection(expected.Value.GeefWerknemerFuncties(),
						item => Assert.Equal(_nf, item));
				});
		}

		[Theory]
		[InlineData(null, "nieuwefunctie")]
		[InlineData("", "nieuwefunctie")]
		[InlineData(" ", "nieuwefunctie")]
		[InlineData("\n", "nieuwefunctie")]
		[InlineData("\r", "nieuwefunctie")]
		[InlineData("\t", "nieuwefunctie")]
		[InlineData("\v", "nieuwefunctie")]
		[InlineData("oudefunctie", null)]
		[InlineData("oudefunctie", "")]
		[InlineData("oudefunctie", " ")]
		[InlineData("oudefunctie", "\n")]
		[InlineData("oudefunctie", "\r")]
		[InlineData("oudefunctie", "\t")]
		[InlineData("oudefunctie", "\v")]
		[InlineData("oudefunctie", "nieuwefunctie")]
		public void WijzigFunctie_Invalid(string oudefunctie, string nieuwefunctie)
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			Assert.Throws<WerknemerException>(() => w.WijzigFunctie(null, _of, _nf));
			Assert.Throws<WerknemerException>(() => w.WijzigFunctie(_b1, oudefunctie, nieuwefunctie));

			//"Werknemer - WijzigFunctie - werknemer is in dit bedrijf niet werkzaam onder deze functie"
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _nf);
			Assert.Throws<WerknemerException>(() => w.WijzigFunctie(_b1, _of, _nf));
			//"Werknemer - WijzigFunctie - werknemer is in dit bedrijf al werkzaam onder deze functie"
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b2, _e, _of);
			Assert.Throws<WerknemerException>(() => w.WijzigFunctie(_b2, _of, _of));
		}
		#endregion

		#region UnitTest Verwijder Functie
		[Fact]
		public void VerwijderFunctie_Valid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _of);
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _nf);
			w.VerwijderFunctie(_b1, _of);
			IReadOnlyDictionary<Bedrijf, WerknemerInfo> actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
			Assert.Collection(actual,
				expected =>
				{
					Assert.Equal(_b1, expected.Key);
					Assert.Equal(_e, expected.Value.Email);
					Assert.Collection(expected.Value.GeefWerknemerFuncties(),
						item => Assert.Equal(_nf, item));
				});
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		[InlineData("oudefunctie")]
		public void VerwijderFunctie_Invalid(string functie)
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			Assert.Throws<WerknemerException>(() => w.VerwijderFunctie(null, _of));
			Assert.Throws<WerknemerException>(() => w.VerwijderFunctie(_b1, functie));
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _of);
			Assert.Throws<WerknemerException>(() => w.VerwijderFunctie(_b1, _nf));
		}
		#endregion

		#region UnitTest Geef bedrijven en functies
		[Fact]
		public void GeefBedrijvenEnFunctiesPerWerknemer_Valid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _nf);
			IReadOnlyDictionary<Bedrijf, WerknemerInfo> actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
			Assert.Collection(actual,
				expected =>
				{
					Assert.Equal(_b1, expected.Key);
					Assert.Equal(_e, expected.Value.Email);
					Assert.Collection(expected.Value.GeefWerknemerFuncties(),
						functie => Assert.Equal(_nf, functie));
				});

			//meerdere functies check
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _e, _of);
			actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
			Assert.Collection(actual,
				expected =>
				{
					Assert.Equal(_b1, expected.Key);
					Assert.Collection(expected.Value.GeefWerknemerFuncties(),
						functie => Assert.Equal(_nf, functie),
						functie => Assert.Equal(_of, functie));
				});
		}
		#endregion

		#region UnitTest Werknemer is gelijk
		[Fact]
		public void WerknemerIsGelijk_Valid()
		{
			Werknemer w = new(10, "werknemer", "werknemersen");
			Assert.True(w.WerknemerIsGelijk(w));
		}

		[Theory]
		[InlineData(1, "werknemer", "werknemersen")]
		[InlineData(10, "remenkrew", "werknemersen")]
		[InlineData(10, "werknemer", "nresnemkrew")]
		public void WerknemerIsGelijk_Invalid(long id, string voornaam, string achternaam)
		{
			Werknemer w1 = new(10, "werknemer", "werknemersen");
			Werknemer w2 = new(id, voornaam, achternaam);
			Assert.False(w1.WerknemerIsGelijk(w2));
		}
		#endregion

		#region UnitTest Werknemer ctor
		[Theory]
		[InlineData(10, "werknemer", "werknemersen", 10, "werknemer", "werknemersen")]

		[InlineData(10, "     werknemer", "werknemersen", 10, "werknemer", "werknemersen")]
		[InlineData(10, "werknemer     ", "werknemersen", 10, "werknemer", "werknemersen")]

		[InlineData(10, "werknemer", "     werknemersen", 10, "werknemer", "werknemersen")]
		[InlineData(10, "werknemer", "werknemersen     ", 10, "werknemer", "werknemersen")]
		public void ctor_Valid(long idIn, string voornaamIn, string achternaamIn, long idUit, string voornaamUit, string achternaamUit)
		{
			Werknemer w = new(idIn, voornaamIn, achternaamIn);
			Assert.Equal(idUit, w.Id);
			Assert.Equal(voornaamUit, w.Voornaam);
			Assert.Equal(achternaamUit, w.Achternaam);
		}

		[Theory]
		[InlineData(0, "werknemer", "werknemersen")]

		[InlineData(10, null, "werknemersen")]
		[InlineData(10, "", "werknemersen")]
		[InlineData(10, " ", "werknemersen")]
		[InlineData(10, "\n", "werknemersen")]
		[InlineData(10, "\r", "werknemersen")]
		[InlineData(10, "\t", "werknemersen")]
		[InlineData(10, "\v", "werknemersen")]

		[InlineData(10, "werknemer", null)]
		[InlineData(10, "werknemer", "")]
		[InlineData(10, "werknemer", " ")]
		[InlineData(10, "werknemer", "\n")]
		[InlineData(10, "werknemer", "\r")]
		[InlineData(10, "werknemer", "\t")]
		[InlineData(10, "werknemer", "\v")]
		public void ctor_Invalid(long id, string voornaam, string achternaam)
		{
			Assert.Throws<WerknemerException>(() => new Werknemer(id, voornaam, achternaam));
		}
		#endregion
	}
}