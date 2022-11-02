﻿using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls {
	/// <summary>
	/// Interaction logic for WerknemersLijstControl.xaml
	/// </summary>
	public partial class WerknemersLijstControl : UserControl {
		public bool HeeftData { get; set; }

		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<WerknemerDTO>),
		  typeof(WerknemersLijstControl),
		  new PropertyMetadata(new ObservableCollection<WerknemerDTO>())
	  );

		public ObservableCollection<WerknemerDTO> ItemSource {
			get { return (ObservableCollection<WerknemerDTO>)GetValue(ItemSourceProperty); }
			set { SetValue(ItemSourceProperty, value); }
		}

		public WerknemersLijstControl() {
			this.DataContext = this;
			InitializeComponent();

			if (!HeeftData) FetchData();
		}

		private void KlikOpActionButtonOpRow(object sender, RoutedEventArgs e) {
			Button? b = sender as Button;
			WerknemerDTO? werknemer = b?.CommandParameter as WerknemerDTO;

			//Zet de geselecteerde werknemer in de DataContext van de BeheerWindow
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

		public void FetchHuidigeAfspraken() {
			FetchData(url: "");
		}

		public void FetchWerknemerAfspraken(WerknemerDTO werknemer) {
			FetchData(url: "", werknemer);
		}

		public void FetchData(string url = "", object body = null) {

		}

		internal void FetchBezoekerAfspraken(BezoekerDTO bezoeker) {
			FetchData(url: "", bezoeker);
		}

		internal void FetchAfsprakenOpDatumData(DateTime now) {
			FetchData(url: "", now);
		}
	}
}
