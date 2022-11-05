﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.ConvertedClasses {
	public class StringDateTime {
		public StringDateTime(DateTime dateTime) {
			_dateTime = dateTime;
		}

		private DateTime _dateTime;
		public string Date { get => _dateTime.ToString("dd/MM/yyyy - HH:mm"); }

		public override string ToString() {
			return Date;
		}
	}
}