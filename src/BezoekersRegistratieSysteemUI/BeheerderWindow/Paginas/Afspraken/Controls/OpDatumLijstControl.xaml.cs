﻿using BezoekersRegistratieSysteemUI.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls {
	public partial class OpDatumLijstControl : UserControl {
		public bool HeeftData { get; set; }

		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<AfspraakDTO>),
		  typeof(OpDatumLijstControl),
		  new PropertyMetadata(new ObservableCollection<AfspraakDTO>())
		 );

		public ObservableCollection<AfspraakDTO> ItemSource {
			get { return (ObservableCollection<AfspraakDTO>)GetValue(ItemSourceProperty); }
			set { SetValue(ItemSourceProperty, value); }
		}

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
		  nameof(SelectedItem),
		  typeof(AfspraakDTO),
		  typeof(OpDatumLijstControl),
		  new PropertyMetadata(null)
		);

		public AfspraakDTO SelectedItem {
			get { return (AfspraakDTO)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
		  nameof(SelectedIndex),
		  typeof(int),
		  typeof(OpDatumLijstControl),
		  new PropertyMetadata(-1)
		);

		public int SelectedIndex {
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public OpDatumLijstControl() {
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

		private void KlikOpAfspraakOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			AfspraakDTO afspraak = (AfspraakDTO)b.CommandParameter;
		}
	}
}