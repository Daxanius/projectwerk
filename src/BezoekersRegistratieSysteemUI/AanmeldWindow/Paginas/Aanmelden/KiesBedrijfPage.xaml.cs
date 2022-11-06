using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.DTO;
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

		/// <summary>
		/// ///////////////////////////////////////////////////
		/// </summary>

		private const int MAX_COLUMN_COUNT = 3;
		private List<BedrijfDTO> _bedrijven;

		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion


		public KiesBedrijfPage() {
			double schermResolutieHeight = System.Windows.SystemParameters.MaximizedPrimaryScreenHeight;
			double schermResolutieWidth = System.Windows.SystemParameters.MaximizedPrimaryScreenWidth;

			double defaultResHeight = 1080.0;
			double defaultResWidth = 1920.0;

			double aspectratio = schermResolutieWidth / schermResolutieHeight;
			double change = aspectratio > 2 ? 0.3 : aspectratio > 1.5 ? 0 : aspectratio > 1 ? -0.05 : -0.3;

			ScaleX = (schermResolutieWidth / defaultResWidth);
			ScaleY = (schermResolutieHeight / defaultResHeight) + change;

			this.DataContext = this;
			InitializeComponent();

			if (_bedrijven is null) {
				FetchAlleBedrijven();
			}
		}

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

		private void GaNaarWerknemersVanBedrijfTab(object sender, MouseButtonEventArgs e) {
			BedrijfDTO geselecteerdbedrijf = (BedrijfDTO)((Border)sender).DataContext;

			Window window = Window.GetWindow(this);
			RegistratieWindow registratieWindow = (RegistratieWindow)window.DataContext;

			RegistratieWindow.GeselecteerdBedrijf = geselecteerdbedrijf;
			registratieWindow.FrameControl.Navigate(new AanmeldGegevensPage());
		}

		#region API Requests
		private async void FetchAlleBedrijven() {
			_bedrijven = new();
			(bool isvalid, List<ApiBedrijfIN> bedrijven) = await ApiController.Get<List<ApiBedrijfIN>>("/bedrijf");

			if (isvalid) {
				bedrijven.ForEach((api) => {
					_bedrijven.Add(new BedrijfDTO(api.Id, api.Naam, api.Btw, api.TelefoonNummer, api.Email, api.Adres));
				});
			} else {
				MessageBox.Show("Er is iets fout gegaan bij het ophalen van de bedrijven", "Error /bedrijf");
			}
			SpawnBedrijvenGrid();
		}
		#endregion
	}
}
