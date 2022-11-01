using BezoekersRegistratieSysteemUI.BeheerderWindow.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.Paginas.Dashboard.Controls {
	/// <summary>
	/// Interaction logic for AfsprakenLijstControl.xaml
	/// </summary>
	public partial class AfsprakenLijstControl : UserControl {
		#region Afspraken
		public ObservableCollection<AfspraakDTO> Afspraken { get; set; }
		#endregion

		public AfsprakenLijstControl() {
			Afspraken = new() {
				new AfspraakDTO(1, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now, DateTime.Now.AddHours(8)),
				new AfspraakDTO(2, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1)),
				new AfspraakDTO(3, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2)),
				new AfspraakDTO(4, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3)),
				new AfspraakDTO(5, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1)),
				new AfspraakDTO(6, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2)),
				new AfspraakDTO(7, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3)),
				new AfspraakDTO(8, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1)),
				new AfspraakDTO(9, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(10,new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)),
				new AfspraakDTO(11, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1), DateTime.Now.AddHours(6)),
				new AfspraakDTO(12, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2)),
				new AfspraakDTO(13, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3)),
				new AfspraakDTO(14, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1)),
				new AfspraakDTO(15, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2)),
				new AfspraakDTO(16, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3)),
				new AfspraakDTO(17, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1)),
				new AfspraakDTO(18, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2)),
				new AfspraakDTO(19, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3)),
				new AfspraakDTO(20, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk") ,DateTime.Now.AddHours(1)),
				new AfspraakDTO(21, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(22, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)) };

			this.DataContext = this;
			InitializeComponent();
		}

		private void KlikOpActionButtonOpRow(object sender, RoutedEventArgs e) {
			Button? b = sender as Button;
			AfspraakDTO? afspraak = b?.CommandParameter as AfspraakDTO;

			OpenAfspraakDetail(afspraak);
		}

		private void OpenAfspraakDetail(AfspraakDTO afspraak) {

		}

		private Border _selecteditem;

		private void KlikOpRow(object sender, MouseButtonEventArgs e) {
			//Er is 2 keer geklikt
			if (e.ClickCount == 2) {
				return;
			}

			if (_selecteditem is not null) {
				_selecteditem.Background = Brushes.Transparent;
				_selecteditem.BorderThickness = new Thickness(0);
			}
			StackPanel? listViewItem = sender as StackPanel;

			Border border = (Border)listViewItem.Children[0];
			border.Background = Brushes.White;
			border.BorderThickness = new Thickness(1);
			border.BorderBrush = Brushes.WhiteSmoke;
			border.CornerRadius = new CornerRadius(20);
			border.Margin = new Thickness(0, 0, 20, 0);
			_selecteditem = border;
		}
	}
}
