﻿using BezoekersRegistratieSysteemUI.Aanmelden.DTO;
using BezoekersRegistratieSysteemUI.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BezoekersRegistratieSysteemUI.Controlls
{
	/// <summary>
	/// Interaction logic for InputControl.xaml
	/// </summary>
	public partial class AfmeldControl : UserControl, INotifyPropertyChanged
	{

		private string _email = "";
		public string Email {
			get {
				return _email;
			}
			set {
				_email = value;
				UpdatePropperty();
			}
		}

		public AfmeldControl()
		{
			this.DataContext = this;
			InitializeComponent();
		}

		#region ProppertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		public void UpdatePropperty([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		//Klik op Ga verder knop
		private void GaVerderButtonClickEvent(object sender, RoutedEventArgs e)
		{
			try
			{
				Email = Email.Trim();
				new MailAddress(Email);
				MeldBezoekerAf(Email);
				ToonPopupBezoekerAfgemeld();
			} catch (Exception)
			{
				MessageBox.Show("Email is niet in een juist formaat", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		private void MeldBezoekerAf(string email)
		{
			//Var body = {
			//	“email”: “”,
			//}
			//[POST] /api/afspraak/end
		}

		private async void ToonPopupBezoekerAfgemeld()
		{
			popupAfgemeld.IsOpen = true;
			afmeldControl.Opacity = .2;
			afmeldControl.IsHitTestVisible = false;

			await Task.Delay(1500);

			popupAfgemeld.IsOpen = false;

			afmeldControl.Opacity = 1;
			afmeldControl.IsHitTestVisible = true;

			GaTerug(null, null);
		}

		private void GaTerug(object sender, MouseButtonEventArgs e)
		{
			Email = string.Empty;

			Window window = Window.GetWindow(this);
			AanOfUitMeldenScherm aanOfUitMeldenScherm = window.DataContext as AanOfUitMeldenScherm;
			aanOfUitMeldenScherm.ResetSchermNaarStart();
		}
	}
}