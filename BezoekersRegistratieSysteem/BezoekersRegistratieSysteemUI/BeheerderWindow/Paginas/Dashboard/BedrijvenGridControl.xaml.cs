using BezoekersRegistratieSysteemUI.BeheerderWindow.DTO;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.Paginas.Dashboard {
	/// <summary>
	/// Interaction logic for BedrijvenGridControl.xaml
	/// </summary>
	public partial class BedrijvenGridControl : UserControl {
		private const int MAX_COLUMN_COUNT = 4;

		public BedrijvenGridControl() {
			this.DataContext = this;
			InitializeComponent();
			SpawnBedrijvenGrid();
		}

		private void SpawnBedrijvenGrid() {
			List<BedrijfDTO> bedrijven = new() { new BedrijfDTO(1, "Hogent", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(2, "Odice", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(3, "Allphi", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(4, "Scheppers", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(5, "Artevelde", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(6, "De Bolster", "Btw5", "Telefoon5", "Email5", "Adress5", null),
				new BedrijfDTO(1, "Brauzz", "Btw", "Telefoon", "Email", "Adress", null),
				new BedrijfDTO(2, "Apple", "Btw1", "Telefoon1", "Email1", "Adress1", null),
				new BedrijfDTO(3, "Microsoft", "Btw2", "Telefoon2", "Email2", "Adress2", null),
				new BedrijfDTO(4, "Hp", "Btw3", "Telefoon3", "Email3", "Adress3", null),
				new BedrijfDTO(5, "Gilo", "Btw4", "Telefoon4", "Email4", "Adress4", null),
				new BedrijfDTO(6, "CCE", "Btw5", "Telefoon5", "Email5", "Adress5", null) };

			int rowCount = 0;
			int columnCount = 0;

			gridContainer.RowDefinitions.Add(new() { Height = new GridLength(1, GridUnitType.Star) });

			for (int i = 0; i < bedrijven.Count; i++) {
				Grid container = new();
				container.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });
				container.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });

				container.Margin = new Thickness(10);
				container.Background = Brushes.Yellow;
				container.HorizontalAlignment = HorizontalAlignment.Center;
				container.VerticalAlignment = VerticalAlignment.Center;
				container.MinHeight = 100;
				container.MinWidth = 75;

				Grid.SetColumn(container, columnCount);
				Grid.SetRow(container, rowCount);

				TextBlock bedrijfNaam = new() {
					Text = bedrijven[i].Naam,
					Background = Brushes.Cyan,
					TextAlignment = TextAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};

				//< iconspresenter:Icon
				//			CircleBackground = "{StaticResource MainAchtergrond}"

				//			IconOffsetLeft = "8"

				//			IconOffsetRight = "8"

				//			IconSize = "40"

				//			IconSource = "../BedrijfIcon.xaml" />

				Grid.SetColumn(bedrijfNaam, 1);
				Grid.SetRow(bedrijfNaam, 0);

				container.Children.Add(bedrijfNaam);

				if (columnCount == MAX_COLUMN_COUNT - 1) {
					columnCount = 0;

					gridContainer.RowDefinitions.Add(new() { Height = new GridLength(1, GridUnitType.Star) });

					rowCount++;
				}
				else {
					columnCount++;
				}

				gridContainer.Children.Add(container);
			}
		}
	}
}
