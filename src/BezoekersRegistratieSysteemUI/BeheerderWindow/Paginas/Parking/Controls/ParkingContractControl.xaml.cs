using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking.Controls {
	public partial class ParkingContractControl : UserControl, INotifyPropertyChanged {

		public ParkingContractControl() {
			this.DataContext = this;
			InitializeComponent();

			//Kijk of je kan rechts klikken om iets te doen
			//NummerplaatLijst.ContextMenuOpening += (sender, args) => args.Handled = true;
			//ContextMenu.ContextMenuClosing += (object sender, ContextMenuEventArgs e) => ContextMenu.DataContext = null;
		}

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}
