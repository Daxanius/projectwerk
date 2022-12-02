using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using BezoekersRegistratieSysteemUI.Events;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven {
	public partial class BedrijvenPage : Page {
		#region Variabelen
		public string Datum => DateTime.Now.ToString("dd.MM");

		private List<BedrijfDTO> initieleBedrijven;
		private string _zoekText;
		public string ZoekText {
			get => _zoekText;
			set {
				if (value.IsNietLeeg()) {
					_zoekText = value.ToLower();

					List<BedrijfDTO> result = initieleBedrijven.Where(b =>
					b.Naam.ToLower().Contains(_zoekText) ||
					b.TelefoonNummer.ToLower().Contains(_zoekText) ||
					b.Adres.ToLower().Contains(_zoekText) ||
					b.Email.ToLower().Contains(_zoekText) ||
					b.BTW.ToLower().Contains(_zoekText)).ToList();

					BedrijvenLijstControl.ItemSource.Clear();

					foreach (BedrijfDTO bedrijf in result) {
						BedrijvenLijstControl.ItemSource.Add(bedrijf);
					}

				} else if (value.Length == 0) {
					BedrijvenLijstControl.ItemSource.Clear();
					foreach (BedrijfDTO bedrijf in ApiController.GeefBedrijven()) {
						BedrijvenLijstControl.ItemSource.Add(bedrijf);
					}
				}
			}
		}
		#endregion

		public BedrijvenPage() {
			this.DataContext = this;
			InitializeComponent();

			BedrijfEvents.NieuwBedrijfToeGevoegd += NieuwBedrijfToeGevoegd_Event;
		}

		private void NieuwBedrijfToeGevoegd_Event(BedrijfDTO bedrijf) {
			BedrijvenLijstControl.ItemSource.Add(bedrijf);
			initieleBedrijven = BedrijvenLijstControl.ItemSource.ToList();
		}

		private void VoegBedrijfToe(object sender, MouseButtonEventArgs e) {
			bedrijfToevoegenPopup.Visibility = Visibility.Visible;
		}

		public void LoadBedrijvenInList(List<BedrijfDTO> bedrijven) {
			foreach (var bedrijf in bedrijven) {
				BedrijvenLijstControl.ItemSource.Add(bedrijf);
			}
			initieleBedrijven = bedrijven;
		}

		private void ZoekTermChanged(object sender, TextChangedEventArgs e) {
			Task.Run(() => Dispatcher.Invoke(() => ZoekText = ZoekTextTextbox.Text));
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
	}
}
