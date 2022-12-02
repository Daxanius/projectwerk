using System;
using System.Globalization;
using System.Windows.Data;

namespace BezoekersRegistratieSysteemUI.ConvertedClasses {
	internal class DashboardStatusConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			string status = value.ToString();
			if (status.ToLower() == "lopend") {
				return status;
			}
			return "Afgerond";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			string status = value.ToString();
			if (status.ToLower() == "lopend") {
				return status;
			}
			return "Afgerond";
		}
	}
}
