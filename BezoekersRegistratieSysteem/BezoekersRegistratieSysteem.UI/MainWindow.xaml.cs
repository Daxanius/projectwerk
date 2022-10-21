﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

namespace BezoekersRegistratieSysteem.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ParkeerButton_Click(object sender, RoutedEventArgs e)
		{
			ParkeerWindow parkeerWindow = new ParkeerWindow();
			this.Close();
			parkeerWindow.ShowDialog();
		}

		private void AanmeldButton_Click(object sender, RoutedEventArgs e)
		{
			AanmeldingsWindow aanmeldWindow = new AanmeldingsWindow();
			this.Close();
			aanmeldWindow.ShowDialog();
		}

		private void AdminButton_Click(object sender, RoutedEventArgs e)
		{
			ParkeerWindow parkeerWindow = new ParkeerWindow();
			this.Close();
			parkeerWindow.ShowDialog();
		}
	}
}
