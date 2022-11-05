using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups {
	/// <summary>
	/// Interaction logic for WerknemersPopup.xaml
	/// </summary>
	public partial class AfsprakenPopup : UserControl, INotifyPropertyChanged {
		public event PropertyChangedEventHandler? PropertyChanged;

		#region Bind Propperties
		private string _werknemer;
		public string Werknemer {
			get { return _werknemer; }
			set {
				if (value == _werknemer) return;
				_werknemer = value;
				UpdatePropperty();
			}
		}
		private string _bezoeker;
		public string Bezoeker {
			get { return _bezoeker; }
			set {
				if (value == _bezoeker) return;
				_bezoeker = value;
				UpdatePropperty();
			}
		}
		private DateTime? _startTijd = null;
		public DateTime? StartTijd {
			get { return _startTijd; }
			set {
				if (value == _startTijd) return;
				_startTijd = value;
				UpdatePropperty();
			}
		}
		private bool _isbeindigd = false;
		public bool IsBeindigd {
			get { return _isbeindigd; }
			set {
				if (value == _isbeindigd) return;
				_isbeindigd = value;
				UpdatePropperty();
			}
		}
		#endregion

		public AfsprakenPopup() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void VoegNieuweFunctieToe(object sender, MouseButtonEventArgs e) {

		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay();
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			//...
			SluitOverlay();
		}

		private void SluitOverlay() {

			AfsprakenPage afsprakenPage = AfsprakenPage.Instance;
			afsprakenPage.AfsprakenPopup.Visibility = Visibility.Hidden;
		}

		#region ProppertyChanged

		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
