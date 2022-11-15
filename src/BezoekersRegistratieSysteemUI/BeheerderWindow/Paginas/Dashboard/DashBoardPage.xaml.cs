using System;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas {
	public partial class DashBoardPage : Page {
		#region Public Propperty
		public static string Datum {
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
		private static readonly object padlock = new();

		public static DashBoardPage Instance {
			get {
				lock (padlock) {
					instance ??= new DashBoardPage();
					return instance;
				}
			}
		}
		#endregion
	}
}
