﻿using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using BezoekersRegistratieSysteemUI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.ParkeerWindow.Paginas.Aanmelden {
	public partial class KiesBedrijfPage : Page {

		#region Variabelen

		private const int MAX_COLUMN_COUNT = 3;

		public List<BedrijfDTO> Bedrijven;
		public List<BedrijfDTO> BedrijvenMetContract = new();

		#endregion

		public KiesBedrijfPage() {
			this.DataContext = this;
			InitializeComponent();

			if (Bedrijven is null) {
				Bedrijven = ApiController.GeefBedrijven().ToList();
				foreach (BedrijfDTO bedrijf in Bedrijven)
				{
					if (ApiController.GeefParkingContract(bedrijf.Id) != null)
					{
                        BedrijvenMetContract.Add(bedrijf);
					}
				}     
				SpawnBedrijvenGrid();
			}
		}

		#region Funcities

		private void SpawnBedrijvenGrid() {
			gridContainer.Children.Clear();
			int rowCount = 0;
			int columnCount = 0;

			gridContainer.RowDefinitions.Add(new() { Height = new GridLength(1, GridUnitType.Star) });

			for (int i = 0; i < BedrijvenMetContract?.Count; i++) {
				Border border = new Border();
				border.Style = Application.Current.Resources["BedrijvenBorderGridStyle"] as Style;
				border.Height = 85;
				border.MinWidth = 350;
				border.Margin = new Thickness(10);
				border.MouseLeftButtonDown += GaNaarWerknemersVanBedrijfTab_Event;
				border.DataContext = BedrijvenMetContract[i];

				StackPanel container = new();
				container.Margin = new Thickness(5);
				container.Orientation = Orientation.Horizontal;
				container.HorizontalAlignment = HorizontalAlignment.Left;
				container.VerticalAlignment = VerticalAlignment.Center;

				TextBlock bedrijfNaam = new() {
					Text = BedrijvenMetContract[i].Naam,
					FontSize = 24,
					FontWeight = FontWeights.Bold,
					TextAlignment = TextAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
					TextWrapping = TextWrapping.Wrap
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

		private void GaNaarWerknemersVanBedrijfTab_Event(object sender, MouseButtonEventArgs e) {
			BedrijfDTO geselecteerdbedrijf = (BedrijfDTO)((Border)sender).DataContext;

			Window window = Window.GetWindow(this);
			AanmeldParkeerWindow aanmeldParkeerWindow = (AanmeldParkeerWindow)window.DataContext;

			AanmeldParkeerWindow.GeselecteerdBedrijf = geselecteerdbedrijf;
			aanmeldParkeerWindow.FrameControl.Content = new AanmeldParkeerPage();
		}

		#endregion

		#region Singleton
		private static KiesBedrijfPage instance = null;
		private static readonly object padlock = new object();

		public static KiesBedrijfPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new KiesBedrijfPage();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
