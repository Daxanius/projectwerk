using System;
using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemUI.Exceptions {
	public class FetchApiException : Exception {
		public FetchApiException() {
		}

		public FetchApiException(string? message) : base(message) {
		}

		public FetchApiException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected FetchApiException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}