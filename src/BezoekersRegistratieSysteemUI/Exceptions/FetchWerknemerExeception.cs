using System;
using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Aanmelden {
	[Serializable]
	internal class FetchWerknemerExeception : Exception {
		public FetchWerknemerExeception() {
		}

		public FetchWerknemerExeception(string? message) : base(message) {
		}

		public FetchWerknemerExeception(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected FetchWerknemerExeception(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}