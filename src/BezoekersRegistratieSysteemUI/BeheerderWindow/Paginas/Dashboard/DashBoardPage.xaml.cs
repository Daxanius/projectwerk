using System;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas {
	public partial class DashBoardPage : Page {
		#region Public Propperty
		public string Datum {
			get {
				return DateTime.Now.ToString("dd.MM");
			}
		}
		#endregion

		public DashBoardPage() {
			this.DataContext = this;
			InitializeComponent();

			//this.NavigationService.Navigate()
			//TODO: :-)
		}

		#region Singleton
		private static DashBoardPage instance = null;
		private static readonly object padlock = new object();

		public static DashBoardPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new DashBoardPage();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
