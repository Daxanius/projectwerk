﻿using System;
using System.Windows.Data;

namespace BezoekersRegistratieSysteemUI.Converter {
	public class StatusConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			return (bool)value ? "Vrij" : "Bezet";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			return (bool)value ? "Vrij" : "Bezet";
		}
	}
}