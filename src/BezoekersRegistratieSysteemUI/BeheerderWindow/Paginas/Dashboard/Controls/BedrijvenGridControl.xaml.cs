using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Dashboard.Controls {
	/// <summary>
	/// Interaction logic for BedrijvenGridControl.xaml
	/// </summary>
	public partial class BedrijvenGridControl : UserControl {
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

		/// <summary>
		/// ///////////////////////////////////////////////////
		/// </summary>
		///

		private const int MAX_COLUMN_COUNT = 4;

		private List<BedrijfDTO> _bedrijven;
		public BedrijvenGridControl() {
			this.DataContext = this;
			InitializeComponent();
			//Acces current thread
			FetchAlleBedrijven();
		}

		private void SpawnBedrijvenGrid() {
			int rowCount = 0;
			int columnCount = 0;

			gridContainer.RowDefinitions.Add(new() { Height = new GridLength(1, GridUnitType.Star) });

			for (int i = 0; i < _bedrijven.Count; i++) {
				Border border = new Border();
				border.Style = Application.Current.Resources["BedrijvenBorderGridStyle"] as Style;
				border.Height = 85;
				border.MinWidth = 300;
				border.Margin = new Thickness(10);
				border.MouseLeftButtonDown += GaNaarWerknemersVanBedrijfTab;
				border.DataContext = _bedrijven[i];

				StackPanel container = new();
				container.Margin = new Thickness(5);
				container.Orientation = Orientation.Horizontal;
				container.HorizontalAlignment = HorizontalAlignment.Left;
				container.VerticalAlignment = VerticalAlignment.Center;

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

		#region API Requests
		private async void FetchAlleBedrijven() {
			_bedrijven = new();
			(bool isvalid, List<BedrijfOutputDTO> bedrijven) = await ApiController.Get<List<BedrijfOutputDTO>>("/bedrijf");

			if (isvalid) {
				bedrijven.ForEach((api) => {
					_bedrijven.Add(new BedrijfDTO(api.Id, api.Naam, api.BTW, api.TelefoonNummer, api.Email, api.Adres));
				});
			} else {
				MessageBox.Show("Er is iets fout gegaan bij het ophalen van de bedrijven", "Error /bedrijf");
			}

			SpawnBedrijvenGrid();
			BedrijvenPage.Instance.LoadBedrijvenInList(_bedrijven);
		}
		#endregion
	}
}
