using System;

namespace VzaarApi
{
	public class VzaarApiException : Exception
	{
		public VzaarApiException () {}

		public VzaarApiException(string message)
			:base(message) {}

		public VzaarApiException(string message, Exception inner)
			:base (message, inner) {}
	}
}