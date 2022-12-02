using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bezoekers {
	public partial class AanwezigeBezoekersPage : Page {
		public AanwezigeBezoekersPage() {
			this.DataContext = this;
			InitializeComponent();
			Task.Run(() => {
				Dispatcher.Invoke(() => {
					List<BezoekerDTO> aanwezigeBezoekers = ApiController.GeefAanwezigeBezoekers().ToList();
					BezoekersLijstControl.ItemSource.Clear();
					aanwezigeBezoekers.ForEach(b => BezoekersLijstControl.ItemSource.Add(b));
				});
			});
		}

		#region Singleton
		private static AanwezigeBezoekersPage instance = null;
		private static readonly object padlock = new object();

		public static AanwezigeBezoekersPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new AanwezigeBezoekersPage();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
