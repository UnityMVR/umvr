using System;
using System.Runtime.Serialization;

namespace pindwin.umvr.Exceptions
{
	public class UMVRException : Exception
	{
		public const string SerializationSetupErrorFormat =
			"Error when trying to deserialize {0}. Expected serializer of type {1}, encountered {2} instead.";
		
		public UMVRException(string message) : base(message)
		{ }

		public UMVRException(string message, Exception innerException) : base(message, innerException)
		{ }

		public UMVRException(SerializationInfo info, StreamingContext context) : base(info, context)
		{ }
	}
}