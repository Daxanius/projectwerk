using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
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

namespace BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Afmelden {
	/// <summary>
	/// Interaction logic for KiesBedrijfPage.xaml
	/// </summary>
	public partial class AfmeldPage : Page {
        #region Scaling
        public double ScaleX { get; set; }
        public double ScaleY { get; set; }
        #endregion


        public AfmeldPage()
        {

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
		}
	}
}
