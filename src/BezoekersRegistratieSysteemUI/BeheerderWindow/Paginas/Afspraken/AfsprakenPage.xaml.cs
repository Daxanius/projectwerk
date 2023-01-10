using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken{
	public partial class AfsprakenPage : Page, INotifyPropertyChanged {
		#region Variabelen
		private static WerknemerDTO? _geselecteerdeWerknemer;

		private int _geselecteerdeHuidigeAfspraakIndex = -1;
		private int _geselecteerdeWerknemerAfspraakIndex = -1;
		private int _geselecteerdeAfspraakOpDatumIndex = -1;

		private bool _huidigeAfsprakenTabIsGeselecteerd = false;
		private bool _afsprakenWerknemerTabIsGeselecteerd = false;
		private bool _afsprakenOpDatumTabIsGeselecteerd = false;

		private HuidigeAfsprakenLijst afsprakenAfsprakenLijstControl = new();
		private WerknemerAfsprakenLijst werknemersAfsprakenLijstControl = new();
		private OpDatumAfsprakenLijstControl opDatumAfsprakenLijstControl = new();

		public BedrijfDTO GeselecteerdBedrijf => BeheerderWindow.GeselecteerdBedrijf;

		public WerknemerDTO GeselecteerdeWerknemer {
			get => _geselecteerdeWerknemer;
			set {
				if (value == null) return;
				_geselecteerdeWerknemer = value;

				UpdateWerknemerAfsprakenOpScherm();
				UpdatePropperty();
			}
		}

        private List<AfspraakDTO> initieleZoekBalkAfsprakenkWerknemers = new();
        private string _zoekTermWerknemersDatum;
        public string ZoekTermWerknemersDatum
        {
            get => _zoekTextWerknemers;
            set
            {
                if (value.IsNietLeeg())
                {
                    _zoekTextWerknemers = value.ToLower();

                    List<AfspraakDTO> result = initieleZoekBalkAfsprakenkWerknemers.Where(a => a.StartTijd.Contains(_zoekTermWerknemersDatum) ||
                    a.StartTijd.Contains(_zoekTermWerknemersDatum)).ToList();

                    GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Clear();

                    foreach (AfspraakDTO afspraak in result)
                    {
                        GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Add(afspraak);
                    }

                }
                else if (value.Length == 0)
                {
                    GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Clear();
                    foreach (AfspraakDTO afspraak in initieleZoekBalkAfsprakenkWerknemers)
                    {
                        GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Add(afspraak);
                    }
                }
            }
        }

        private List<AfspraakDTO> initieleZoekBalkAfspraken = new();
        private string _zoekTermDatum;
        public string ZoekTermDatum
        {
            get => _zoekTermDatum;
            set
            {
                if (value.IsNietLeeg())
                {
                    _zoekTermDatum = value.ToLower();

                    List<AfspraakDTO> result = initieleZoekBalkAfspraken.Where(a => a.StartTijd.Contains(_zoekTermDatum) || 
					a.StartTijd.Contains(_zoekTermDatum)).ToList();

                    OpDatumAfsprakenLijst.ItemSource.Clear();

                    foreach (AfspraakDTO afspraak in result)
                    {
                        OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
                    }

                }
                else if (value.Length == 0)
                {
                    OpDatumAfsprakenLijst.ItemSource.Clear();
                    foreach (AfspraakDTO afspraak in initieleZoekBalkAfspraken)
                    {
                        OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
                    }
                }
            }
        }

        private List<WerknemerDTO> initieleZoekBalkWerknemers = new();
		private string _zoekTextWerknemers;
		public string ZoekTextWerknemers {
			get => _zoekTextWerknemers;
			set {
				if (value.IsNietLeeg()) {
					_zoekTextWerknemers = value.ToLower();

					List<WerknemerDTO> result = initieleZoekBalkWerknemers.Where(w => w.Voornaam.ToLower().Contains(_zoekTextWerknemers) ||
					w.Achternaam.ToLower().Contains(_zoekTextWerknemers) ||
					w.Email.ToLower().Contains(_zoekTextWerknemers) ||
					w.Functie.ToLower().Contains(_zoekTextWerknemers) ||
					w.Status.ToString().ToLower().Contains(_zoekTextWerknemers)).ToList();

					WerknemerLijst.ItemSource.Clear();

					foreach (WerknemerDTO werknemer in result) {
						WerknemerLijst.ItemSource.Add(werknemer);
					}

				} else if (value.Length == 0) {
					WerknemerLijst.ItemSource.Clear();
					foreach (WerknemerDTO werknemer in initieleZoekBalkWerknemers) {
						WerknemerLijst.ItemSource.Add(werknemer);
					}
				}
			}
		}

		#endregion

		public AfsprakenPage() {
			this.DataContext = this;
			InitializeComponent();

			UpdateGeselecteerdBedrijf_Event();
			NavigeerNaarTab("Huidige Afspraken");

			//Events
			GlobalEvents.RefreshData += AutoUpdateIntervalAfspraken_Event;
			BedrijfEvents.GeselecteerdBedrijfChanged += UpdateGeselecteerdBedrijf_Event;
			AfspraakEvents.NieuweAfspraakToegevoegd += NieuweAfspraakToegevoegd_Event;
			AfspraakEvents.UpdateAfspraak += UpdatedAfspraak_Event;
			WerknemerEvents.NieuweWerknemerToegevoegd += NieuweWerknemerToegevoegd_Event;
			WerknemerEvents.UpdateWerknemer += UpdateWerknemer_Event;
		}

		#region Functies
		private void ZoekTermChangedWerknemers(object sender, TextChangedEventArgs e) => Task.Run(() => Dispatcher.Invoke(() => ZoekTextWerknemers = ZoekTextTextBoxWerknemers.Text));
        private void ZoekTermKeyDownWerknemersDatum(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ZoekTermWerknemersDatum = ZoekTermTextBoxWerknemersDatum.Text;
            }
        }

        private void ZoekTermKeyDownDatum(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ZoekTermDatum = ZoekTermTextBoxDatum.Text;
            }
        }

        private void AutoUpdateIntervalAfspraken_Event() {
			if (_huidigeAfsprakenTabIsGeselecteerd)
				UpdateHuidigeAfsprakenOpScherm();
			else if (_afsprakenWerknemerTabIsGeselecteerd)
				UpdateWerknemerAfsprakenOpScherm();
			else if (_afsprakenOpDatumTabIsGeselecteerd)
				UpdateOpDatumAfsprakenOpScherm();
		}
		private void UpdateGeselecteerdBedrijf_Event() {
			afsprakenAfsprakenLijstControl.ItemSource.Clear();
			werknemersAfsprakenLijstControl.ItemSource.Clear();
			opDatumAfsprakenLijstControl.ItemSource.Clear();

			WerknemerLijst.ItemSource.Clear();
			OpDatumAfsprakenLijst.ItemSource.Clear();

			ResetFilterSelection();

			afsprakenAfsprakenLijstControl.HeeftData = false;
			werknemersAfsprakenLijstControl.HeeftData = false;

			NavigeerNaarTab("Huidige Afspraken");
			UpdatePropperty(nameof(GeselecteerdBedrijf));
			UpdateHuidigeAfsprakenOpScherm();
		}
		private void NieuweAfspraakToegevoegd_Event(AfspraakDTO afspraak) {
			if (ZoekTermTextBoxDatum != null && afspraak.StartTijdDate.Day == int.Parse(ZoekTermDatum.Substring(0,2))) {
				OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
			}

			// Als de afspraak nog bezig is voegen we hem toe aan de huidige
			// afspraken lijst
			if (afspraak.EindTijd.IsLeeg()) {
				List<AfspraakDTO> afspraken = HuidigeAfsprakenLijst.ItemSource.ToList();
				afspraken.Add(afspraak);
				int index = afspraken.OrderByDescending(a => a.StartTijd).ThenByDescending(a => a.Bezoeker.Voornaam).ToList().IndexOf(afspraak);
				HuidigeAfsprakenLijst.ItemSource.Insert(index, afspraak);
			}
		}
		private void UpdatedAfspraak_Event(AfspraakDTO afspraak) {
			if (afspraak is null) return;

			if (ZoekTermTextBoxDatum != null && afspraak.StartTijdDate.Day == int.Parse(ZoekTermDatum.Substring(0, 2))) {
				int index = HuidigeAfsprakenLijst.ItemSource.IndexOf(afspraak);
				if (index == -1) return;
				HuidigeAfsprakenLijst.ItemSource.RemoveAt(index);
				if (afspraak.EindTijd.IsLeeg())
					HuidigeAfsprakenLijst.ItemSource.Insert(index, afspraak);

				index = OpDatumAfsprakenLijst.ItemSource.IndexOf(afspraak);
				if (index == -1) return;
				OpDatumAfsprakenLijst.ItemSource.RemoveAt(index);
				OpDatumAfsprakenLijst.ItemSource.Insert(index, afspraak);
			}
		}
		private void NieuweWerknemerToegevoegd_Event(WerknemerDTO werknemer) {
			if (werknemer is null) return;
			WerknemerLijst.ItemSource.Add(werknemer);
			initieleZoekBalkWerknemers.Add(werknemer);
		}
		private void UpdateWerknemer_Event(WerknemerDTO werknemer) {
			if (werknemer is null) return;
			var werknemerLijstWerknemer = WerknemerLijst.ItemSource.FirstOrDefault(w => w.Id == werknemer.Id);
			var initieleZoekBalkWerknemersWerknemer = initieleZoekBalkWerknemers.FirstOrDefault(w => w.Id == werknemer.Id);
			var huidigeAfsprakenWerknemer = HuidigeAfsprakenLijst.ItemSource.Where(a => a.Werknemer.Id == werknemer.Id).ToList();
			var afsprakenOpDatumWerknemer = OpDatumAfsprakenLijst.ItemSource.Where(a => a.Werknemer.Id == werknemer.Id).ToList();
			var werknemerAfsprakenWerknemer = GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Where(a => a.Werknemer.Id == werknemer.Id).ToList();

			int werknemerLijstWerknemerIndex = WerknemerLijst.ItemSource.IndexOf(werknemerLijstWerknemer);
			int initieleZoekBalkWerknemersWerknemerIndex = initieleZoekBalkWerknemers.IndexOf(initieleZoekBalkWerknemersWerknemer);

			for (int i = 0; i < huidigeAfsprakenWerknemer.Count(); i++) {
				var afspraak = huidigeAfsprakenWerknemer[i];
				int index = HuidigeAfsprakenLijst.ItemSource.IndexOf(afspraak);
				if (index > -1) {
					HuidigeAfsprakenLijst.ItemSource.RemoveAt(index);
					afspraak.Werknemer = werknemer;
					if (afspraak.EindTijdDate is null)
						HuidigeAfsprakenLijst.ItemSource.Insert(index, afspraak);
				}
			}

			for (int i = 0; i < afsprakenOpDatumWerknemer.Count(); i++) {
				var afspraak = afsprakenOpDatumWerknemer[i];
				int index = OpDatumAfsprakenLijst.ItemSource.IndexOf(afspraak);
				if (index > -1) {
					OpDatumAfsprakenLijst.ItemSource.RemoveAt(index);
					afspraak.Werknemer = werknemer;
					OpDatumAfsprakenLijst.ItemSource.Insert(index, afspraak);
				}
			}

			for (int i = 0; i < werknemerAfsprakenWerknemer.Count(); i++) {
				var afspraak = werknemerAfsprakenWerknemer[i];
				int index = GeselecteerdeWerknemerAfsprakenLijst.ItemSource.IndexOf(afspraak);
				if (index > -1) {
					GeselecteerdeWerknemerAfsprakenLijst.ItemSource.RemoveAt(index);
					afspraak.Werknemer = werknemer;
					GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Insert(index, afspraak);
				}
			}

			if (werknemerLijstWerknemerIndex != -1) {
				WerknemerLijst.ItemSource.RemoveAt(werknemerLijstWerknemerIndex);
				WerknemerLijst.ItemSource.Insert(werknemerLijstWerknemerIndex, werknemer);
			}

			if (initieleZoekBalkWerknemersWerknemerIndex != -1) {
				initieleZoekBalkWerknemers.RemoveAt(initieleZoekBalkWerknemersWerknemerIndex);
				initieleZoekBalkWerknemers.Insert(initieleZoekBalkWerknemersWerknemerIndex, werknemer);
			}
		}

		private void UpdateOpDatumAfsprakenOpScherm() {
			if (OpDatumAfsprakenLijst.SelectedItem is AfspraakDTO) {
				_geselecteerdeAfspraakOpDatumIndex = OpDatumAfsprakenLijst.SelectedIndex;
			}

			if (GeselecteerdBedrijf is not null) {
				OpDatumAfsprakenLijst.ItemSource.Clear();
				foreach (AfspraakDTO afspraak in ApiController.GeefAfsprakenOpDatumVanBedrijf(GeselecteerdBedrijf.Id, ZoekTermDatum).OrderByDescending(a => a.StartTijd)) {
					OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
				}
				initieleZoekBalkAfspraken = new(OpDatumAfsprakenLijst.ItemSource);
			}

			if (_geselecteerdeAfspraakOpDatumIndex != -1) {
				OpDatumAfsprakenLijst.SelectedIndex = _geselecteerdeAfspraakOpDatumIndex;
			}
		}
		private void UpdateWerknemerAfsprakenOpScherm() {
			if (_geselecteerdeWerknemer is not null && GeselecteerdBedrijf is not null) {
				if (GeselecteerdeWerknemerAfsprakenLijst.SelectedItem is AfspraakDTO) {
					_geselecteerdeWerknemerAfspraakIndex = GeselecteerdeWerknemerAfsprakenLijst.SelectedIndex;
				}

				GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Clear();
				foreach (AfspraakDTO afspraak in ApiController.GeefWerknemerAfsprakenVanBedrijf(GeselecteerdBedrijf.Id, GeselecteerdeWerknemer, ZoekTermWerknemersDatum).OrderByDescending(a => a.StartTijd)) {
					GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Add(afspraak);
				}

				if (_geselecteerdeWerknemerAfspraakIndex != -1) {
					GeselecteerdeWerknemerAfsprakenLijst.SelectedIndex = _geselecteerdeWerknemerAfspraakIndex;
				}
			}
		}
		private void UpdateHuidigeAfsprakenOpScherm() {
			if (HuidigeAfsprakenLijst.SelectedItem is AfspraakDTO) {
				_geselecteerdeHuidigeAfspraakIndex = HuidigeAfsprakenLijst.SelectedIndex;
			}

			HuidigeAfsprakenLijst.ItemSource.Clear();
			foreach (AfspraakDTO afspraak in ApiController.GeefAfsprakenVanBedrijf(GeselecteerdBedrijf.Id).OrderByDescending(a => a.StartTijd).ThenByDescending(a => a.Bezoeker.Voornaam)) {
				if (afspraak.StartTijdDate.Day == DateTime.Now.Day && afspraak.EindTijd.IsLeeg()) {
					HuidigeAfsprakenLijst.ItemSource.Add(afspraak);
				}
			}

			if (_geselecteerdeHuidigeAfspraakIndex != -1) {
				HuidigeAfsprakenLijst.SelectedIndex = _geselecteerdeHuidigeAfspraakIndex;
			}
		}

		private void Navigeer_Click(object sender, MouseButtonEventArgs e) {
			TextBlock textBlock = (TextBlock)((StackPanel)((Border)sender).Child).Children[1];

			switch (textBlock.Text) {
				case "Huidige Afspraken":
				NavigeerNaarTab("Huidige Afspraken");
				UpdateHuidigeAfsprakenOpScherm();
				break;

				case "Afspraken Werknemer":
				NavigeerNaarTab("Afspraken Werknemer");
				if (!werknemersAfsprakenLijstControl.HeeftData) {
					foreach (WerknemerDTO werknemer in ApiController.GeefWerknemersVanBedrijf(GeselecteerdBedrijf).OrderByDescending(a => a.Voornaam)) {
						WerknemerLijst.ItemSource.Add(werknemer);
					}
					werknemersAfsprakenLijstControl.HeeftData = true;
					initieleZoekBalkWerknemers = WerknemerLijst.ItemSource.ToList();
                    initieleZoekBalkAfsprakenkWerknemers = GeselecteerdeWerknemerAfsprakenLijst.ItemSource.ToList();
                    }
				break;

				case "Afspraak Op Datum":
				NavigeerNaarTab("Afspraak Op Datum");
				UpdateOpDatumAfsprakenOpScherm();
                break;
			}
		}
		private void OpenAfsprakenPopup_Click(object sender, MouseButtonEventArgs e) {
			afsprakenPopup.StartTijd = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
			afsprakenPopup.Visibility = Visibility.Visible;
		}

		private void NavigeerNaarTab(string tabIndex) {
			ResetFilterSelection();
			switch (tabIndex) {
				case "Huidige Afspraken":
				_huidigeAfsprakenTabIsGeselecteerd = true;
				FilterContainerHeaders.Children[0].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[0].Visibility = Visibility.Visible;
				break;

				case "Afspraken Werknemer":
				_afsprakenWerknemerTabIsGeselecteerd = true;
				FilterContainerHeaders.Children[1].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[1].Visibility = Visibility.Visible;
				break;

				case "Afspraak Op Datum":
				_afsprakenOpDatumTabIsGeselecteerd = true;
				FilterContainerHeaders.Children[2].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[2].Visibility = Visibility.Visible;
				break;
			}
		}
		private void ResetFilterSelection() {
			for (int i = 0; i < FilterContainerHeaders.Children.Count; i++) {
				FilterContainerHeaders.Children[i].Opacity = .6;
				((Grid)FilterContainer.Children[0]).Children[i].Visibility = Visibility.Collapsed;
			}

			_huidigeAfsprakenTabIsGeselecteerd = false;
			_afsprakenWerknemerTabIsGeselecteerd = false;
			_afsprakenOpDatumTabIsGeselecteerd = false;

			GeselecteerdeWerknemerAfsprakenLijst.AfsprakenLijst.SelectedIndex = -1;
			GeselecteerdeWerknemerAfsprakenLijst.AfsprakenLijst.SelectedItem = null;
			GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Clear();
			WerknemerLijst.SelectedIndex = -1;
			HuidigeAfsprakenLijst.SelectedIndex = -1;
			OpDatumAfsprakenLijst.SelectedIndex = -1;
		}
		#endregion

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged

		#region Singleton
		private static AfsprakenPage instance = null;
		private static readonly object padlock = new object();

		public static AfsprakenPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new AfsprakenPage();
					}
					return instance;
				}
			}
		}
        #endregion
    }
}
