using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Popups;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven {
	/// <summary>
	/// Interaction logic for DashBoardPage.xaml
	/// </summary>
	public partial class BedrijvenPage : Page {
		#region Public Propperty
		public static string Datum {
			get {
				return DateTime.Now.ToString("dd.MM");
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
		}

		private void VoegBedrijfToe(object sender, MouseButtonEventArgs e) {
			bedrijvenPopup.Visibility = Visibility.Visible;
		}

		public void LoadBedrijvenInList(List<BedrijfDTO> bedrijven) {
			foreach (var bedrijf in bedrijven) {
				BedrijvenLijstControl.ItemSource.Add(bedrijf);
			}
		}


		#region Singleton
		private static BedrijvenPage instance = null;
		private static readonly object padlock = new();

		public static BedrijvenPage Instance {
			get {
				lock (padlock) {
					instance ??= new BedrijvenPage();
					return instance;
				}
			}
		}
		#endregion
	}
}
