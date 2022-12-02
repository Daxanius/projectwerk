namespace BezoekerRegistratieSysteemDLPicker {
	public class DLPickerException : Exception {
		public DLPickerException(string? message) : base(message) {
		}

		public DLPickerException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
