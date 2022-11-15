using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BezoekersRegistratieSysteemUI.ConvertedClasses {
	internal class ElipsisConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			int lengte = 20;
			if (parameter is not null) lengte = int.Parse((string)parameter);
			string input = value.ToString();
			if (input.Trim().Length > lengte) {
				input = input[..lengte];
				input += "...";
			}
			return input;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			int lengte = 20;
			if (parameter is not null) lengte = int.Parse((string)parameter);
			string input = value.ToString();
			if (input.Trim().Length > lengte) {
				input = input[..lengte];
				input += "...";
			}
			return input;
		}
	}
}
