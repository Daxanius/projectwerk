using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Aanmelden {
	/// <summary>
	/// Interaction logic for KiesBedrijfPage.xaml
	/// </summary>
	public partial class KiesBedrijfPage : Page {
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

		#region Private Fields

		private const int MAX_COLUMN_COUNT = 3;
		public List<BedrijfDTO> _bedrijven;

		#endregion

		public KiesBedrijfPage() {
			this.DataContext = this;
			InitializeComponent();

			if (_bedrijven is null) {
				FetchAlleBedrijven();
			}
		}

		#region Fetch Bedrijven

		private void SpawnBedrijvenGrid() {
			int rowCount = 0;
			int columnCount = 0;

			gridContainer.RowDefinitions.Add(new() { Height = new GridLength(1, GridUnitType.Star) });

			for (int i = 0; i < _bedrijven?.Count; i++) {
				Border border = new Border();
				border.Style = Application.Current.Resources["BedrijvenBorderGridStyle"] as Style;
				border.Height = 85;
				border.MinWidth = 350;
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

		#endregion

		#region Navigate

		private void GaNaarWerknemersVanBedrijfTab(object sender, MouseButtonEventArgs e) {
			BedrijfDTO geselecteerdbedrijf = (BedrijfDTO)((Border)sender).DataContext;

			Window window = Window.GetWindow(this);
			RegistratieWindow registratieWindow = (RegistratieWindow)window.DataContext;

			RegistratieWindow.GeselecteerdBedrijf = geselecteerdbedrijf;
			registratieWindow.FrameControl.Content = new AanmeldGegevensPage();
		}

		#endregion

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
				return;
			}
			SpawnBedrijvenGrid();
		}
		#endregion
	}
}
