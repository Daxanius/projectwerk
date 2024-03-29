using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domeinen {
	public class UnitTestAfspraak {
		//AF

		#region Valid Info
		private readonly Bezoeker _b;
		private readonly Werknemer _w;
		private readonly Bedrijf _bd;
		private readonly DateTime _st;
		private readonly DateTime _et;
		#endregion

		#region Invalid Info
		private readonly Bezoeker _ivb;
		private readonly Werknemer _ivw;
		private readonly Bedrijf _ivbd;
		#endregion

		#region Initialiseren
		public UnitTestAfspraak() {
			_b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			_w = new(10, "werknemer", "werknemersen");
			_bd = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			_st = DateTime.Now;
			_et = _st.AddHours(2);

			_ivb = new(1, "anderebezoeker", "anderebezoekersen", "anderebezoeker.bezoekersen@email.com", "anderbezoekerbedrijf");
			_ivw = new(1, "anderewerknemer", "anderewerknemersen");
			_ivbd = new(1, "anderbedrijf", "BE0676747521", true, "876543210", "anderbedrijf@email.com", "anderbedrijfstraat 10");

			//For Valid Paths
			_bd.VoegWerknemerToeInBedrijf(_w, "werknemer.werknemersen@email.com", "functie");
			//

			//For Invalid Paths
			_ivbd.VoegWerknemerToeInBedrijf(_ivw, "anderewerknemer.werknemersen@email.com", "anderefunctie");
			//
		}
		#endregion

		#region UnitTest Id
		[Fact]
		public void ZetId_Valid() {
			Afspraak a = new(10, _st, null, _bd, _b, _w);
			a.ZetId(10);
			Assert.Equal((long)10, a.Id);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		public void ZetId_Invalid(long id) {
			Afspraak a = new(10, _st, null, _bd, _b, _w);
			Assert.Throws<AfspraakException>(() => a.ZetId(id));
		}
		#endregion

		#region UnitTest Starttijd
		[Fact]
		public void ZetStarttijd_Valid() {
			Afspraak a = new(10, _st, null, _bd, _b, _w);
			a.ZetStarttijd(_st);
			Assert.Equal(_st, a.Starttijd);
		}

		[Fact]
		public void ZetStarttijd_Invalid() {
			Afspraak a = new(10, _st, _et, _bd, _b, _w);
			//"Afspraak - ZetStarttijd - Afspraak is al afgelopen"
			Assert.Throws<AfspraakException>(() => a.ZetStarttijd(_st));
			Assert.Throws<AfspraakException>(() => a.ZetStarttijd(new DateTime()));

		}
		#endregion

		#region UnitTest Eindtijd
		[Fact]
		public void ZetEindtijd_Valid() {
			Afspraak a = new(10, _st, null, _bd, _b, _w);
			//Null check
			a.ZetEindtijd(null);
			Assert.Null(a.Eindtijd);
			//Eindtijd check
			a.ZetEindtijd(_et);
			Assert.Equal(_et, a.Eindtijd);
		}

		[Fact]
		public void ZetEindtijd_Invalid() {
			Afspraak a = new(10, _et, null, _bd, _b, _w);
			//"Afspraak - ZetEindtijd - Eindtijd moet na starttijd liggen"
			Assert.Throws<AfspraakException>(() => a.ZetEindtijd(_st));
		}
		#endregion

		#region UnitTest Bezoeker
		[Fact]
		public void ZetBezoeker_Valid() {
			Afspraak a = new(10, _st, _et, _bd, _b, _w);

			a.ZetBezoeker(_b);
			Assert.Equal(_b, a.Bezoeker);
		}

		[Fact]
		public void ZetBezoeker_Invalid() {
			Afspraak a = new(10, _st, _et, _bd, _b, _w);
			//"Afspraak - ZetBezoeker - Bezoeker mag niet leeg zijn"
			Assert.Throws<AfspraakException>(() => a.ZetBezoeker(null));
		}
		#endregion

		#region UnitTest Werknemer
		[Fact]
		public void ZetWerknemer_Valid() {
			Afspraak a = new(10, _st, _et, _bd, _b, _w);

			a.ZetWerknemer(_w);
			Assert.Equal(_w, a.Werknemer);
		}

		[Fact]
		public void ZetWerknemer_Invalid() {
			Afspraak a = new(10, _st, _et, _bd, _b, _w);
			//"Afspraak - ZetWerknemer - Werknemer mag niet leeg zijn"
			Assert.Throws<AfspraakException>(() => a.ZetWerknemer(null));
		}
		#endregion

		#region UnitTest Afspraak Gelijk
		[Fact]
		public void AfspraakIsGelijk_Valid() {
			Afspraak a = new(10, _st, null, _bd, _b, _w);
			Assert.True(a.AfspraakIsGelijk(a));
		}

		[Fact]
		public void AfspraakIsGelijk_Invalid() {
			Afspraak a1 = new(10, _st, null, _bd, _b, _w);
			Afspraak a2 = new(1, _st.AddHours(1), _et, _ivbd, _ivb, _ivw);

			Assert.False(a1.AfspraakIsGelijk(a2));
		}
		#endregion

		#region UnitTest Afspraak ctor BL
		[Fact]
		public void BL_ctor_Valid() {
			Afspraak a = new(_st, _bd, _b, _w);

			Assert.Equal(_st, a.Starttijd);
			Assert.Equal(_bd, a.Bedrijf);
			Assert.Equal(_b, a.Bezoeker);
			Assert.Equal(_w, a.Werknemer);
		}

		[Fact]
		public void BL_ctor_Invalid_NullOfDefaultStarttijd() {
			Assert.Throws<AfspraakException>(() => new Afspraak(new DateTime(), _bd, _b, _w));
		}

		[Fact]
		public void BL_ctor_Invalid_BedrijfLeeg() {
			Assert.Throws<AfspraakException>(() => new Afspraak(_st, null, _b, _w));
		}

		[Fact]
		public void BL_ctor_Invalid_BezoekerLeeg() {
			Assert.Throws<AfspraakException>(() => new Afspraak(_st, _bd, null, _w));
		}

		[Fact]
		public void BL_ctor_Invalid_WerknemerLeeg() {
			Assert.Throws<AfspraakException>(() => new Afspraak(_st, _bd, _b, null));
		}

		[Fact]
		public void BL_ctor_Invalid_ctorLeeg() {
			Assert.Throws<AfspraakException>(() => new Afspraak(new DateTime(), null, null, null));
		}
		#endregion

		#region UnitTest Afspraak ctor DL
		[Fact]
		public void DL_ctor_Valid_EindtijfNull() {
			Afspraak a = new(10, _st, null, _bd, _b, _w);

			Assert.Equal((long)10, a.Id);
			Assert.Equal(_st, a.Starttijd);

			//Eindtijd Null check
			Assert.Null(a.Eindtijd);

			Assert.Equal(_b, a.Bezoeker);
			Assert.Equal(_w, a.Werknemer);
		}

		public void DL_ctor_Valid_EindtijdIngevuld() {
			Afspraak a = new(10, _st, null, _bd, _b, _w);

			Assert.Equal((long)10, a.Id);
			Assert.Equal(_st, a.Starttijd);

			//Eindtijd ingevuld check
			a.ZetEindtijd(_et);
			Assert.Equal(_et, a.Eindtijd);

			Assert.Equal(_b, a.Bezoeker);
			Assert.Equal(_w, a.Werknemer);
		}

		[Fact]
		public void DL_ctor_Invalid_Id0() {
			//Id 0
			Assert.Throws<AfspraakException>(() => new Afspraak(0, _st, null, _bd, _b, _w));
		}

		[Fact]
		public void DL_ctor_Invalid_IdNegatief() {
			//Id 0
			Assert.Throws<AfspraakException>(() => new Afspraak(-1, _st, null, _bd, _b, _w));
		}

		[Fact]
		public void DL_ctor_Invalid_NullOfDefaultStarttijd() {
			Assert.Throws<AfspraakException>(() => new Afspraak(10, new DateTime(), null, _bd, _b, _w));
		}

		[Fact]
		public void DL_ctor_Invalid_EindtijdVoorStartijd() {
			Assert.Throws<AfspraakException>(() => new Afspraak(10, _et, _st, _bd, _b, _w));
		}

		[Fact]
		public void DL_ctor_Invalid_BedrijfLeeg() {
			Assert.Throws<AfspraakException>(() => new Afspraak(10, _st, _et, null, _b, _w));
		}

		[Fact]
		public void DL_ctor_Invalid_BezoekerLeeg() {
			Assert.Throws<AfspraakException>(() => new Afspraak(10, _st, _et, _bd, null, _w));
		}

		[Fact]
		public void DL_ctor_Invalid_WerknemerLeeg() {
			Assert.Throws<AfspraakException>(() => new Afspraak(10, _st, _et, _bd, _b, null));
		}

		[Fact]
		public void DL_ctor_Invalid_ctorLeeg() {
			Assert.Throws<AfspraakException>(() => new Afspraak(10, new DateTime(), null, null, null, null));
		}
		#endregion
	}
}