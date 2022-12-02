namespace BezoekersRegistratieSysteemUI.MessageBoxes {
	public interface ICustomMessageBox {
		public ECustomMessageBoxResult Show(string message, string title, ECustomMessageBoxIcon icon);
	}
}
