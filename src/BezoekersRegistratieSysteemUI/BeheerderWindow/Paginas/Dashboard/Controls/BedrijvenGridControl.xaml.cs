﻿using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Popups;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Dashboard.Controls {
	/// <summary>
	/// Interaction logic for BedrijvenGridControl.xaml
	/// </summary>
	public partial class BedrijvenGridControl : UserControl {
		private static BedrijvenGridControl instance = null;
		private static readonly object padlock = new();

		public static BedrijvenGridControl Instance {
			get {
				lock (padlock) {
					instance ??= new BedrijvenGridControl();
					return instance;
				}
			}
		}

		private const int MAX_COLUMN_COUNT = 4;
		private readonly List<BedrijfDTO> _bedrijven;

		public BedrijvenGridControl() {
			this.DataContext = this;
			InitializeComponent();

			BedrijvenPopup.UpdateBedrijfLijst += UpdateListMetBedrijf;

			_bedrijven = ApiController.FetchBedrijven().ToList();

			SpawnBedrijvenGrid();
			BedrijvenPage.LoadBedrijvenInList(_bedrijven);
		}

		private void UpdateListMetBedrijf(BedrijfDTO bedrijf) {
			_bedrijven.Add(bedrijf);
			SpawnBedrijvenGrid();
		}

		private void SpawnBedrijvenGrid() {
			gridContainer.Children.Clear();
			int rowCount = 0;
			int columnCount = 0;

			gridContainer.RowDefinitions.Add(new() { Height = new GridLength(1, GridUnitType.Star) });

			for (int i = 0; i < _bedrijven.Count; i++) {
				Border border = new() {
					Style = Application.Current.Resources["BedrijvenBorderGridStyle"] as Style,
					Height = 85,
					MinWidth = 300,
					Margin = new Thickness(10)
				};
				border.MouseLeftButtonDown += GaNaarWerknemersVanBedrijfTab;
				border.DataContext = _bedrijven[i];

				StackPanel container = new() {
					Margin = new Thickness(5),
					Orientation = Orientation.Horizontal,
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Center
				};

				TextBlock bedrijfNaam = new() {
					Text = _bedrijven[i].Naam,
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
			BeheerderWindow.GeselecteerdBedrijf = bedrijf;

			beheerderWindow.FrameControl.Content = AfsprakenPage.Instance;
			beheerderWindow.SideBar.AfsprakenTab.Tag = "Selected";

			((TextBlock)((StackPanel)beheerderWindow.SideBar.AfsprakenTab.Child).Children[1]).FontWeight = FontWeights.Bold;
			((TextBlock)((StackPanel)beheerderWindow.SideBar.AfsprakenTab.Child).Children[1]).Opacity = 1;
			((Icon)((StackPanel)beheerderWindow.SideBar.AfsprakenTab.Child).Children[0]).Opacity = 1;

			((TextBlock)((StackPanel)beheerderWindow.SideBar.WerknemersTab.Child).Children[1]).Opacity = 1;
			((Icon)((StackPanel)beheerderWindow.SideBar.WerknemersTab.Child).Children[0]).Opacity = 1;
		}
	}
}
