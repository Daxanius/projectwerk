﻿using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven {
	/// <summary>
	/// Interaction logic for DashBoardPage.xaml
	/// </summary>
	public partial class BedrijvenPage : Page {
		#region Public Propperty
		public string Datum {
			get {
				return DateTime.Now.ToString("dd.MM");
			}
		}
		#endregion

		public BedrijvenPage() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void VoegBedrijfToe(object sender, MouseButtonEventArgs e) {
			BedrijvenPopup.Visibility = Visibility.Visible;
		}

		public void LoadBedrijvenInList(List<BedrijfDTO> bedrijven) {
			BedrijvenLijst.BedrijvenGrid.DataContext = bedrijven;
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
