using System;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.ConvertedClasses {
	public class StringDateTime {
		public StringDateTime(DateTime dateTime) {
			_dateTime = dateTime;
		}

		private readonly DateTime _dateTime;
		public string Date { get => _dateTime.ToString("dd/MM/yyyy - HH:mm"); }

		public override string ToString() {
			return Date;
		}
	}
}
