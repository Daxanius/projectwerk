using BezoekersRegistratieSysteemUI.AanmeldWindow;
using BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Aanmelden;
using BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Afmelden;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
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

namespace BezoekersRegistratieSysteemUI.AanmeldenOfAfmeldenWindow.Aanmelden.Paginas {
	/// <summary>
	/// Interaction logic for SideBarControl.xaml
	/// </summary>
	public partial class SideBarControlRegistratie : UserControl {
		public SideBarControlRegistratie() {
			InitializeComponent();

			foreach (Border border in BorderContainer.Children) {
				((Icon)((StackPanel)border.Child).Children[0]).Opacity = .6;
			}
		}

		private void VeranderTab(object sender, MouseButtonEventArgs e) {
			string tab = (string)((Label)((StackPanel)((Border)sender).Child).Children[1]).Content;

			foreach (Border border in BorderContainer.Children) {
				border.Tag = "UnSelected";
				((Label)((StackPanel)(border).Child).Children[1]).FontWeight = FontWeights.Normal;
				((Icon)((StackPanel)border.Child).Children[0]).Opacity = .6;
			}

			((Border)sender).Tag = "Selected";
			((Label)((StackPanel)((Border)sender).Child).Children[1]).FontWeight = FontWeights.Bold;
			((Icon)((StackPanel)((Border)sender).Child).Children[0]).Opacity = 1;


			Window window = Window.GetWindow(this);
			RegistratieWindow registratieWindow = (RegistratieWindow)window.DataContext;

			switch (tab) {
				case "Aanmelden":
				registratieWindow.FrameControl.Content = KiesBedrijfPage.Instance;
				break;
				case "Afmelden":
				registratieWindow.FrameControl.Content = AfmeldPage.Instance;
				break;
			}
		}

		private void VraagHulpKnop(object sender, MouseButtonEventArgs e) {
			MessageBox.Show("Moeilijkheden met aanmelden?\n\n Gelieve zich tot de baliemedewerker te richten.", "HELP", MessageBoxButton.OK, MessageBoxImage.Question);
		}
	}
}
