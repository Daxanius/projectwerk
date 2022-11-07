using BezoekersRegistratieSysteemREST.Model.Output;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers {
	/// <summary>
	/// Interaction logic for DashBoardPage.xaml
	/// </summary>
	public partial class WerknemersPage : Page, INotifyPropertyChanged {
		private static WerknemersPage instance = null;
		private static readonly object padlock = new object();

		public static WerknemersPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new WerknemersPage();
					}
					return instance;
				}
			}
		}

		/// <summary>
		/// ///////////////////////////////////////////////////
		/// </summary>

		public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }

		public int FullWidth { get; set; }
		public int FullHeight { get; set; }

		public WerknemersPage() {
			FullWidth = (int)SystemParameters.PrimaryScreenWidth;
			FullHeight = (int)SystemParameters.PrimaryScreenHeight;

			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;

			this.DataContext = this;
			InitializeComponent();

			FetchAlleWerknemers();
		}

		private void UpdateGeselecteerdBedrijfOpScherm() {
			UpdatePropperty(nameof(GeselecteerdBedrijf));
			FetchAlleWerknemers();
		}

		private void AddWerknemer(object sender, MouseButtonEventArgs e) {
			WerknemersPopup.Visibility = Visibility.Visible;
		}

		#region API Requests
		private async void FetchAlleWerknemers() {
			WerknemerLijstControl.ItemSource.Clear();
			(bool isvalid, List<WerknemerOutputDTO> apiWerknemers) = await ApiController.Get<List<WerknemerOutputDTO>>("werknemer/bedrijf/id/" + BeheerderWindow.GeselecteerdBedrijf.Id);
			if (isvalid) {
				apiWerknemers.ForEach((api) => {
					List<WerknemerInfoDTO> lijstWerknemerInfo = new(api.WerknemerInfo.Select(w => new WerknemerInfoDTO(BeheerderWindow.GeselecteerdBedrijf, w.Email, w.Functies)).ToList());
					WerknemerInfoOutputDTO werknemerInfo = api.WerknemerInfo.First(w => w.Bedrijf.Id == BeheerderWindow.GeselecteerdBedrijf.Id);
					WerknemerLijstControl.ItemSource.Add(new WerknemerDTO(api.Id, api.Voornaam, api.Achternaam, werknemerInfo.Email, werknemerInfo.Functies, true));
				});
			} else {
				MessageBox.Show("Er is iets fout gegaan bij het ophalen van de bedrijven", "Error /bedrijf");
			}
		}
		#endregion

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}
