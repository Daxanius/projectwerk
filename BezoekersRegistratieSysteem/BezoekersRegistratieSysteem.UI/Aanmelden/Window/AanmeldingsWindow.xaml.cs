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
using System.Windows.Shapes;

namespace BezoekersRegistratieSysteem.UI
{
	/// <summary>
	/// Interaction logic for AanmeldWindow.xaml
	/// </summary>
	public partial class AanmeldingsWindow : Window
	{
		public delegate void StartGegevensFlowEvent();
		public static event StartGegevensFlowEvent? StartGegevensFlow;

		private static string _bedrijfsNaam = "";
		public static string BedrijfsNaam {
			get => _bedrijfsNaam;
			set {
				_bedrijfsNaam = value;
				StartGegevensFlow?.Invoke();
			}
		}

		public AanmeldingsWindow()
		{
			StartGegevensFlow += GaNaarGegevensControl;
			InitializeComponent();
		}

		private void GaNaarGegevensControl()
		{
			kiesBedrijfControl.Visibility = Visibility.Collapsed;
			gegevensControl.Visibility = Visibility.Visible;
		}
	}
}
