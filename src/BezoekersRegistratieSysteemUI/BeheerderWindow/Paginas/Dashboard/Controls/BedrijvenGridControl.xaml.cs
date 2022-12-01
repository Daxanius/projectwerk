using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Popups;
using BezoekersRegistratieSysteemUI.Events;
using System.Reflection;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Dashboard.Controls {
	public partial class BedrijvenGridControl : UserControl {
		private const int MAX_COLUMN_COUNT = 4;
		private List<BedrijfDTO> _bedrijvenLijst;

		public BedrijvenGridControl() {
			this.DataContext = this;
			InitializeComponent();

			BedrijfEvents.NieuwBedrijfToeGevoegd += NieuwBedrijfToeGevoegd_Event;
			BedrijfEvents.BedrijfVerwijderd += BedrijfVerwijderd_Event;
			BedrijfEvents.BedrijfGeupdate += BedrijfGeUpdate_Event;

			_bedrijvenLijst = ApiController.GeefBedrijven().ToList();

			SpawnBedrijvenGrid();
			BedrijvenPage.Instance.LoadBedrijvenInList(_bedrijvenLijst);
		}

		private void BedrijfVerwijderd_Event(BedrijfDTO bedrijf) {
			_bedrijvenLijst.Remove(bedrijf);
			SpawnBedrijvenGrid();
		}

		private void NieuwBedrijfToeGevoegd_Event(BedrijfDTO bedrijf) {
			_bedrijvenLijst.Add(bedrijf);
			SpawnBedrijvenGrid();
		}

		private void BedrijfGeUpdate_Event(BedrijfDTO bedrijf) {
			if (bedrijf is null) return;

			for (int i = 0; i < gridContainer.Children.Count; i++) {
				Border border = (Border)gridContainer.Children[i];
				if (border.DataContext is BedrijfDTO bedrijfDTO) {
					if (bedrijfDTO.Id == bedrijf.Id) {
						border.DataContext = bedrijf;
						StackPanel stackPanel = (StackPanel)border.Child;
						TextBlock textBlock = (TextBlock)stackPanel.Children[1];
						textBlock.Text = bedrijf.Naam;
						break;
					}
				}
			}
		}

		private void SpawnBedrijvenGrid() {
			gridContainer.Children.Clear();
			int rowCount = 0;
			int columnCount = 0;

			gridContainer.RowDefinitions.Add(new() { Height = new GridLength(1, GridUnitType.Star) });

			for (int i = 0; i < _bedrijvenLijst.Count; i++) {
				Border border = new Border();
				border.Style = Application.Current.Resources["BedrijvenBorderGridStyle"] as Style;
				border.Height = 85;
				border.MinWidth = 300;
				border.Margin = new Thickness(10);
				border.MouseLeftButtonDown += GaNaarWerknemersVanBedrijfTab_Event;
				border.DataContext = _bedrijvenLijst[i];

				StackPanel container = new();
				container.Margin = new Thickness(5);
				container.Orientation = Orientation.Horizontal;
				container.HorizontalAlignment = HorizontalAlignment.Left;
				container.VerticalAlignment = VerticalAlignment.Center;

				TextBlock bedrijfNaam = new() {
					Text = _bedrijvenLijst[i].Naam,
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

		private void GaNaarWerknemersVanBedrijfTab_Event(object sender, MouseButtonEventArgs e) {
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

			((TextBlock)((StackPanel)beheerderWindow.SideBar.ParkingTab.Child).Children[1]).Opacity = 1;
			((Icon)((StackPanel)beheerderWindow.SideBar.ParkingTab.Child).Children[0]).Opacity = 1;
		}

		#region Singleton
		private static BedrijvenGridControl instance = null;
		private static readonly object padlock = new object();

		public static BedrijvenGridControl Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new BedrijvenGridControl();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
