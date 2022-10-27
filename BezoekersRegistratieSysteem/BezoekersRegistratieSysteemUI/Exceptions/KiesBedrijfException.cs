using System;
using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemUI.Exceptions
{
	public class KiesBedrijfException : Exception
	{
		public KiesBedrijfException()
		{
		}

		public KiesBedrijfException(string? message) : base(message)
		{
		}

		public KiesBedrijfException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected KiesBedrijfException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}