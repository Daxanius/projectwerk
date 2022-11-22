using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.MessageBoxes {
	public interface ICustomMessageBox {
		public ECustomMessageBoxResult Show(string message, string title, ECustomMessageBoxIcon icon);
	}
}
