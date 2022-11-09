using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas {
	public partial class DashBoardPage : Page {
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
	}
}
