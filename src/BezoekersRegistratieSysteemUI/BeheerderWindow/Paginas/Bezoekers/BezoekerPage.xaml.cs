using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
