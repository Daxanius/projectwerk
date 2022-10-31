﻿using System;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.Paginas.Afspraken {
	/// <summary>
	/// Interaction logic for DashBoardPage.xaml
	/// </summary>
	public partial class AfsprakenPage : Page {
		#region Public Propperty
		public string Datum {
			get {
				return DateTime.Now.ToString("dd.MM");
			}
		}
		#endregion

		public AfsprakenPage() {
			this.DataContext = this;
			InitializeComponent();
		}
	}
}