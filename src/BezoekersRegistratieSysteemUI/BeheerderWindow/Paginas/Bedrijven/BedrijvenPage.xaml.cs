using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven {
	public partial class BedrijvenPage : Page {
		#region Variabelen
		public string Datum => DateTime.Now.ToString("dd.MM");

		private List<BedrijfDTO> initieleBedrijven;
		private string _zoekText;
		public string ZoekText {
			get => _zoekText;
			set {
				if (!string.IsNullOrWhiteSpace(value)) {
					_zoekText = value;

					List<BedrijfDTO> result = initieleBedrijven.Where(b => 
					b.Naam.Contains(_zoekText) || 
					b.TelefoonNummer.Contains(_zoekText) || 
					b.Adres.Contains(_zoekText) || 
					b.Email.Contains(_zoekText) || 
					b.BTW.Contains(_zoekText)).ToList();

					BedrijvenLijstControl.ItemSource.Clear();

					foreach (BedrijfDTO bedrijf in result) {
						BedrijvenLijstControl.ItemSource.Add(bedrijf);
					}
					
				} else if (value.Length == 0) {
					BedrijvenLijstControl.ItemSource.Clear();
					foreach (BedrijfDTO bedrijf in ApiController.FetchBedrijven()) {
						BedrijvenLijstControl.ItemSource.Add(bedrijf);
					}
				}
			}
		}
		#endregion

		public BedrijvenPage() {
			this.DataContext = this;
			InitializeComponent();

			BedrijvenPopup.UpdateBedrijfLijst += UpdateBedrijvenLijst;
		}

		private void UpdateBedrijvenLijst(BedrijfDTO bedrijf) {
			BedrijvenLijstControl.ItemSource.Add(bedrijf);
			initieleBedrijven = BedrijvenLijstControl.ItemSource.ToList();
		}

		private void VoegBedrijfToe(object sender, MouseButtonEventArgs e) {
			bedrijvenPopup.Visibility = Visibility.Visible;
		}

		public void LoadBedrijvenInList(List<BedrijfDTO> bedrijven) {
			foreach (var bedrijf in bedrijven) {
				BedrijvenLijstControl.ItemSource.Add(bedrijf);
			}
			initieleBedrijven = bedrijven;
		}

		#region Singleton
		private static BedrijvenPage instance = null;
		private static readonly object padlock = new object();

		public static BedrijvenPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new BedrijvenPage();
					}
					return instance;
				}
			}
		}
		#endregion

		private void ZoekTermChanged(object sender, TextChangedEventArgs e) {
			Task.Run(() => Dispatcher.Invoke(() => ZoekText = ZoekTextTextbox.Text));
		}
	}
}
