using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domeinen
{
	public class UnitTestAfspraak
	{
        //AF

        #region Valid Info
        private Bezoeker _b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
        private Werknemer _w = new(10, "werknemer", "werknemersen");
        private static  DateTime _st = DateTime.Now;
        private static DateTime _et = _st.AddHours(2);
        #endregion

        #region UnitTest Id
        [Fact]
        public void ZetId_Valid()
        {
            Afspraak a = new(10, _st, null, _b, _w);
            a.ZetId(10);
            Assert.Equal((uint)10, a.Id);
        }

        [Fact]
        public void ZetId_Invalid()
        {
            Afspraak a = new(10, _st, null, _b, _w);
            Assert.Throws<AfspraakException>(() => a.ZetId(0));
        }
        #endregion

        #region UnitTest Starttijd
        [Fact]
        public void ZetStarttijd_Valid()
        {
            Afspraak a = new(10, _st, null, _b, _w);
            a.ZetStarttijd(_st);
            Assert.Equal(_st, a.Starttijd);
        }
        
        [Fact]
        public void ZetStarttijd_Invalid()
        {
            Afspraak a = new(10, _st, _et, _b, _w);
            //"Afspraak - ZetStarttijd - Afspraak is al afgelopen"
            Assert.Throws<AfspraakException>(() => a.ZetStarttijd(_st));
            Assert.Throws<AfspraakException>(() => a.ZetStarttijd(new DateTime()));

        }
        #endregion

        #region UnitTest Eindtijd
        [Fact]
        public void ZetEindtijd_Valid()
        {
            Afspraak a = new(10, _st, null, _b, _w);
            //Null check
            a.ZetEindtijd(null);
            Assert.Null(a.Eindtijd);
            //Eindtijd check
            a.ZetEindtijd(_et);
            Assert.Equal(_et, a.Eindtijd);
        }

        [Fact]
        public void ZetEindtijd_Invalid()
        {
            Afspraak a = new(10, _et, null, _b, _w);
            //"Afspraak - ZetEindtijd - Eindtijd moet na starttijd liggen"
            Assert.Throws<AfspraakException>(() => a.ZetEindtijd(_st));
        }
        #endregion

        #region UnitTest Bezoeker
        [Fact]
		public void ZetBezoeker_Valid()
		{
            Afspraak a = new(10, _st, _et, _b, _w);

            a.ZetBezoeker(_b);
            Assert.Equal(_b, a.Bezoeker);
		}

        [Fact]
        public void ZetBezoeker_Invalid()
        {
            Afspraak a = new(10, _st, _et, _b, _w);
            //"Afspraak - ZetBezoeker - Bezoeker mag niet leeg zijn"
            Assert.Throws<AfspraakException>(() => a.ZetBezoeker(null));
        }
        #endregion

        #region UnitTest Werknemer
        [Fact]
        public void ZetWerknemer_Valid()
        {
            Afspraak a = new(10, _st, _et, _b, _w);

            a.ZetWerknemer(_w);
            Assert.Equal(_w, a.Werknemer);
        }

        [Fact]
        public void ZetWerknemer_Invalid()
        {
            Afspraak a = new(10, _st, _et, _b, _w);
            //"Afspraak - ZetWerknemer - Werknemer mag niet leeg zijn"
            Assert.Throws<AfspraakException>(() => a.ZetWerknemer(null));
        }
        #endregion

        #region UnitTest Afspraak Gelijk
        [Fact]
        public void AfspraakIsGelijk_Valid()
        {
            Afspraak a = new(10, _st, null, _b, _w);
            Assert.True(a.AfspraakIsGelijk(a));
        }

        [Fact]
        public void AfspraakIsGelijk_Invalid()
        {
            Afspraak a1 = new(10, _st, null, _b, _w);
            Afspraak a2 = new(1, _st.AddHours(1), _et, new(1, "anderebezoeker", "anderebezoekersen", "anderebezoeker.bezoekersen@email.com", "anderbezoekerbedrijf"), new(1, "anderewerknemer", "andere werknemersen"));
            
            Assert.False(a1.AfspraakIsGelijk(a2));
        }
        #endregion

        #region UnitTest Afspraak ctor
        [Fact]
        public void ctor_Valid()
        {
            Afspraak a = new(10, _st, null, _b, _w);

            Assert.Equal((uint)10, a.Id);
            Assert.Equal(_st, a.Starttijd);

            //Eindtijd Null check
            Assert.Null(a.Eindtijd);

            //Eindtijd ingevuld check
            a.ZetEindtijd(_et);
            Assert.Equal(_et, a.Eindtijd);

            Assert.Equal(_b, a.Bezoeker);
            Assert.Equal(_w, a.Werknemer);
        }

        [Fact]
        public void ctor_Invalid()
        {
            //Id 0
            Assert.Throws<AfspraakException>(() => new Afspraak(0, _st, null, _b, _w));
            //Null/Default check Starttijd
            Assert.Throws<AfspraakException>(() => new Afspraak(10, new DateTime(), null, _b, _w));
            //Eindtijd voor Starttijd
            Assert.Throws<AfspraakException>(() => new Afspraak(10, _et, _st, _b, _w));
            //Bezoeker is Null
            Assert.Throws<AfspraakException>(() => new Afspraak(10, _st, _et, null, _w));
            //Werknemer is Null
            Assert.Throws<AfspraakException>(() => new Afspraak(10, _st, _et, _b, null));
            //Constructor leeg
            Assert.Throws<AfspraakException>(() => new Afspraak(10, new DateTime(), null, null, null));
        }
        #endregion
    }
}