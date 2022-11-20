using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Model;
using System;
using System.Collections.Generic;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking
{
    /// <summary>
    /// Interaction logic for ParkingPage.xaml
    /// </summary>
    public partial class ParkingPage : Page, INotifyPropertyChanged
    {
        public int FullWidth { get; set; }
        public int FullHeight { get; set; }
        public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }
        public ParkingPage()
        {
            FullWidth = (int)SystemParameters.PrimaryScreenWidth;
            FullHeight = (int)SystemParameters.PrimaryScreenHeight;

            BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;

            this.DataContext = this;
            InitializeComponent();
        }

        private void UpdateGeselecteerdBedrijfOpScherm()
        {
            UpdatePropperty(nameof(GeselecteerdBedrijf));
        }

        #region Singleton
        private static ParkingPage instance = null;
        private static readonly object padlock = new object();

        public static ParkingPage Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ParkingPage();
                    }
                    return instance;
                }
            }
        }
        #endregion

        #region ProppertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void UpdatePropperty([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion ProppertyChanged
    }
}
