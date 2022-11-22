using BezoekersRegistratieSysteemUI.ParkeerWindow;
using BezoekersRegistratieSysteemUI.ParkeerWindow.Paginas.Aanmelden;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.ParkeerWindow.Paginas
{
	/// <summary>
	/// Interaction logic for SideBarControl.xaml
	/// </summary>
	public partial class SideBarControlRegistratie : UserControl {
		public SideBarControlRegistratie() {
			InitializeComponent();
		}

		private void VraagHulpKnop(object sender, MouseButtonEventArgs e) {
			MessageBox.Show("Lukt aanmelden niet?\n\n Neem contact op met de beheerder aan de balie.", "Probleem", MessageBoxButton.OK, MessageBoxImage.Question);
		}
	}
}
