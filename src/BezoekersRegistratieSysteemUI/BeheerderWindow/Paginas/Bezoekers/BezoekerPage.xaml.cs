using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bezoekers {
	public partial class BezoekerPage : Page {
		public BezoekerPage() {
			this.DataContext = this;
			InitializeComponent();
			List<BezoekerDTO> aanwezigeBezoekers = ApiController.GeefAanwezigeBezoekers().ToList();
			BezoekersLijstControl.ItemSource.Clear();
			aanwezigeBezoekers.ForEach(b => BezoekersLijstControl.ItemSource.Add(b));
		}

		#region Singleton
		private static BezoekerPage instance = null;
		private static readonly object padlock = new object();

		public static BezoekerPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new BezoekerPage();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
