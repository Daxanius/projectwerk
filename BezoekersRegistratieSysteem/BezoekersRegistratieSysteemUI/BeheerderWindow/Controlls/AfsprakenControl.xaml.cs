using BezoekersRegistratieSysteemUI.BeheerderWindow.Controlls.DetailControls;
using BezoekersRegistratieSysteemUI.BeheerderWindow.DTO;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.Controlls {

	/// <summary>
	/// Interaction logic for AfsprakenControl.xaml
	/// </summary>
	public partial class AfsprakenControl : UserControl {
		public ObservableCollection<AfspraakDTO> Afspraken { get; set; }

		public AfsprakenControl() {
			Afspraken = new() {
				new AfspraakDTO(1, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now, DateTime.Now.AddHours(8)),
				new AfspraakDTO(2, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1), DateTime.Now.AddHours(6)),
				new AfspraakDTO(3, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(4, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)),
				new AfspraakDTO(5, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1), DateTime.Now.AddHours(6)),
				new AfspraakDTO(6, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(7, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)),
				new AfspraakDTO(8, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1), DateTime.Now.AddHours(6)),
				new AfspraakDTO(9, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(10,new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)),
				new AfspraakDTO(11, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1), DateTime.Now.AddHours(6)),
				new AfspraakDTO(12, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(13, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)),
				new AfspraakDTO(14, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1), DateTime.Now.AddHours(6)),
				new AfspraakDTO(15, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(16, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)),
				new AfspraakDTO(17, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(1), DateTime.Now.AddHours(6)),
				new AfspraakDTO(18, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(19, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)),
				new AfspraakDTO(20, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk") ,DateTime.Now.AddHours(1), DateTime.Now.AddHours(6)),
				new AfspraakDTO(21, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(22, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), new WerknemerDTO("Weude", "VanDirk"), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)) };

			this.DataContext = this;
			InitializeComponent();
		}

		private void klikOpActionButtonOpRow(object sender, RoutedEventArgs e) {
			Button? b = sender as Button;
			AfspraakDTO? afspraak = b?.CommandParameter as AfspraakDTO;

			OpenAfspraakDetail(afspraak);
		}

		private void OpenAfspraakDetail(AfspraakDTO afspraak) {
			AfspraakDetailWindow afspraakDetailWindow = new AfspraakDetailWindow(afspraak);
			afspraakDetailWindow.Show();
		}

		private StackPanel _selecteditem;

		private void KlikOpRow(object sender, MouseButtonEventArgs e) {
			//Er is 2 keer geklikt
			if (e.ClickCount == 2) {
				return;
			}

			if (_selecteditem is not null)
				_selecteditem.Background = null;
			StackPanel? listViewItem = sender as StackPanel;
			listViewItem.Background = (Brush)new BrushConverter().ConvertFrom("#F0F0F0");
			_selecteditem = listViewItem;
		}
	}
}