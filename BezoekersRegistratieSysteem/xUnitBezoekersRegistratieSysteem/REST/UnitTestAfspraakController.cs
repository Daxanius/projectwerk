using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Controllers;
using Moq;

namespace xUnitBezoekersRegistratieSysteem.REST {
	public class UnitTestAfspraakController {
		#region MOQ
		// Moq repos
		private Mock<IAfspraakRepository> _mockRepoAfspraak;
		private Mock<IBedrijfRepository> _mockRepoBedrijf;
		private Mock<IWerknemerRepository> _mockRepoWerknemer;

		// Managers
		private AfspraakManager _afspraakManager;
		private BedrijfManager _bedrijfManager;
		private WerknemerManager _werknemerManger;

		// Controllers
		private AfspraakController _afspraakController;
		#endregion

		#region Valid Info
		private DateTime _st;
		private DateTime _et;
		private Bezoeker _b;
		private Werknemer _w;

		private Bedrijf _bd;

		private Afspraak _ia;
		private Afspraak _oa;
		#endregion

		#region Initialiseren
		public UnitTestAfspraakController() {
			_st = DateTime.Now;
			_et = _st.AddHours(2);

			_b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			_w = new(10, "werknemer", "werknemersen");

			_bd = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			_bd.VoegWerknemerToeInBedrijf(_w, "werknemer.werknemersen@email.com", "functie");

			_ia = new(_st, _bd, _b, _w);
			_oa = new(10, _st, _et, _bd, _b, _w);

			// Moq repos
			_mockRepoAfspraak = new();
			_mockRepoBedrijf = new();
			_mockRepoWerknemer = new();

			// Managers
			_afspraakManager = new(_mockRepoAfspraak.Object);
			_bedrijfManager = new(_mockRepoBedrijf.Object);
			_werknemerManger = new(_mockRepoWerknemer.Object);

			// Controllers
			_afspraakController = new(_afspraakManager , _werknemerManger, _bedrijfManager);
		}
		#endregion
	}
}
