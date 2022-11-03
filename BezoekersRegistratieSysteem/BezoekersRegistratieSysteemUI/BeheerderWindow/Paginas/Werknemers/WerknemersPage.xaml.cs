using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers {
	/// <summary>
	/// Interaction logic for DashBoardPage.xaml
	/// </summary>
	public partial class WerknemersPage : Page {
		#region Public Propperty
		public ObservableCollection<WerknemerDTO> WerknemersVanGeselecteerdBedrijf { get; set; } = new();

		public BedrijfDTO GeselecteerdBedrijf { get; set; }
		#endregion

		public WerknemersPage() {
			GeselecteerdBedrijf = BeheerderWindow.GeselecteerdBedrijf;

			BedrijfDTO bedrijf = new(1, "Hogent", "Btw", "Telnummer", "Email", "Adres", null);

			WerknemerDTO medewerkerMetFuncties = new WerknemerDTO(1, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, false);
			WerknemerDTO medewerkerMetFuncties2 = new WerknemerDTO(2, "Weude2", "VanDirk2", "Weude2@VanDirk.be", bedrijf, false);
			medewerkerMetFuncties.Functies.AddRange(new List<string>() { "CEO", "CFO" });
			medewerkerMetFuncties2.Functies.AddRange(new List<string>() { "CEO" });

			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(1, "Stan", "Persoons", "Stan.Persoons@student.hogent.be", bedrijf, false));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(2, "Stan1", "Persoons1", "Stan1.Persoons@student.hogent.be", bedrijf, false));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(3, "Stan2", "Persoons2", "Stan2.Persoons@student.hogent.be", bedrijf, true));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(4, "Stan3", "Persoons3", "Stan3.Persoons@student.hogent.be", bedrijf, true));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(5, "Stan4", "Persoons4", "Stan4.Persoons@student.hogent.be", bedrijf, true));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(6, "Stan5", "Persoons5", "Stan5.Persoons@student.hogent.be", bedrijf, false));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(7, "Stan6", "Persoons6", "Stan6.Persoons@student.hogent.be", bedrijf, true));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(8, "Stan7", "Persoons7", "Stan7.Persoons@student.hogent.be", bedrijf, false));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(9, "Stan8", "Persoons8", "Stan8.Persoons@student.hogent.be", bedrijf, false));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(10, "Stan9", "Persoons9", "Stan9.Persoons@student.hogent.be", bedrijf, false));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(11, "Stan10", "Persoons10", "Stan10.Persoons@student.hogent.be", bedrijf, false));
			WerknemersVanGeselecteerdBedrijf.Add(new WerknemerDTO(12, "Stan11", "Persoons11", "Stan11.Persoons@student.hogent.be", bedrijf, false));
			WerknemersVanGeselecteerdBedrijf.Add(medewerkerMetFuncties);
			WerknemersVanGeselecteerdBedrijf.Add(medewerkerMetFuncties2);

			this.DataContext = this;
			InitializeComponent();

			WerknemerLijstControl.ItemSource = WerknemersVanGeselecteerdBedrijf;
		}
	}
}
