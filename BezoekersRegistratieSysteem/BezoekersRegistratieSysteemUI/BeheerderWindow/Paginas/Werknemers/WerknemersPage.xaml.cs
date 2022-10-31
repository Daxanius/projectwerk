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

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.Paginas.Werknemers {
	/// <summary>
	/// Interaction logic for DashBoardPage.xaml
	/// </summary>
	public partial class WerknemersPage : Page {
		#region Public Propperty
		public string Datum {
			get {
				return DateTime.Now.ToString("dd.MM");
			}
		}
		#endregion

		public WerknemersPage() {
			this.DataContext = this;
			InitializeComponent();
		}
	}
}
