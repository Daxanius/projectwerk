﻿using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using MahApps.Metro.Markup;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Dashboard.Controls {
	/// <summary>
	/// Interaction logic for BedrijvenGridControl.xaml
	/// </summary>
	public partial class BedrijvenGridControl : UserControl {
		private const int MAX_COLUMN_COUNT = 4;

		private List<BedrijfDTO> bedrijven = new() { new BedrijfDTO(1, "Hogent", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(2, "Odice", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(3, "Allphi", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(4, "Scheppers", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(5, "Artevelde", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(6, "De Bolster", "Btw5", "Telefoon5", "Email5", "Adress5", null),
				new BedrijfDTO(7, "Brauzz", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(8, "Apple", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(9, "Microsoft", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(10, "Hp", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(11, "Gilo", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(12, "CCE", "Btw5", "Telefoon5", "Email5", "Adress5", null),
				new BedrijfDTO(13, "Hogent", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(14, "Odice", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(15, "Allphi", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(16, "Scheppers", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(17, "Artevelde", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(18, "De Bolster", "Btw5", "Telefoon5", "Email5", "Adress5", null),
				new BedrijfDTO(19, "Brauzz", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(20, "Apple", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(21, "Microsoft", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(22, "Hp", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(23, "Gilo", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(24, "CCE", "Btw5", "Telefoon5", "Email5", "Adress5", null),
				new BedrijfDTO(1, "Hogent", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(2, "Odice", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(3, "Allphi", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(4, "Scheppers", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(5, "Artevelde", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(6, "De Bolster", "Btw5", "Telefoon5", "Email5", "Adress5", null),
				new BedrijfDTO(7, "Brauzz", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(8, "Apple", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(9, "Microsoft", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(10, "Hp", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(11, "Gilo", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(12, "CCE", "Btw5", "Telefoon5", "Email5", "Adress5", null),
				new BedrijfDTO(13, "Hogent", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(14, "Odice", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(15, "Allphi", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(16, "Scheppers", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(17, "Artevelde", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(18, "De Bolster", "Btw5", "Telefoon5", "Email5", "Adress5", null),
				new BedrijfDTO(19, "Brauzz", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(20, "Apple", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(21, "Microsoft", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(22, "Hp", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(23, "Gilo", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(24, "CCE", "Btw5", "Telefoon5", "Email5", "Adress5", null),};

		public BedrijvenGridControl() {
			this.DataContext = this;
			InitializeComponent();
			SpawnBedrijvenGrid();
		}

		private void SpawnBedrijvenGrid() {
			int rowCount = 0;
			int columnCount = 0;

			gridContainer.RowDefinitions.Add(new() { Height = new GridLength(1, GridUnitType.Star) });

			for (int i = 0; i < bedrijven.Count; i++) {
				Border border = new Border();
				border.Style = Application.Current.Resources["BedrijvenBorderGridStyle"] as Style;
				border.Height = 85;
				border.MinWidth = 300;
				border.Margin = new Thickness(10);
				border.MouseLeftButtonDown += GaNaarWerknemersVanBedrijfTab;
				border.DataContext = bedrijven[i];

				StackPanel container = new();
				container.Margin = new Thickness(5);
				container.Orientation = Orientation.Horizontal;
				container.HorizontalAlignment = HorizontalAlignment.Left;
				container.VerticalAlignment = VerticalAlignment.Center;

				TextBlock bedrijfNaam = new() {
					Text = bedrijven[i].Naam,
					FontSize = 24,
					FontWeight = FontWeights.Medium,
					TextAlignment = TextAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
					TextWrapping = TextWrapping.Wrap,
					Padding = new Thickness(0, 0, 0, 4)
				};

				Icon icon = new() {
					IconSize = 42,
					CircleSize = 48,
					IconOffsetLeft = 12,
					IconOffsetTop = 12,
					IconSource = "../BedrijfIcon.xaml",
					Margin = new Thickness(10, 0, 10, 0)
				};

				Grid.SetColumn(border, columnCount);
				Grid.SetRow(border, rowCount);

				container.Children.Add(icon);
				container.Children.Add(bedrijfNaam);

				border.Child = container;

				if (columnCount == MAX_COLUMN_COUNT - 1) {
					columnCount = 0;

					gridContainer.RowDefinitions.Add(new() { Height = new GridLength(1, GridUnitType.Star) });

					rowCount++;
				} else {
					columnCount++;
				}

				gridContainer.Children.Add(border);
			}
		}

		private void GaNaarWerknemersVanBedrijfTab(object sender, MouseButtonEventArgs e) {
			BedrijfDTO bedrijf = (BedrijfDTO)((Border)sender).DataContext;

			Window window = Window.GetWindow(this);
			BeheerderWindow beheerderWindow = (BeheerderWindow)window.DataContext;

			beheerderWindow.ZetGeselecteerdBedrijf(bedrijf);
			beheerderWindow.FrameControl.Navigate(new AfsprakenPage());
		}
	}
}
