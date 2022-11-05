using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Converter {
	public class StatusConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			return (bool)value ? "Vrij" : "Bezet";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			return (bool)value ? "Vrij" : "Bezet";
		}
	}
}
