using System;
using System.Runtime.Serialization;
using pindwin.development;

namespace pindwin.umvr.Exceptions
{
	public class UMVRInvalidOperationException : UMVRException
	{
		private const string Format = "Invalid operation: object of type {0} already disposed.";

		public UMVRInvalidOperationException(Type type)
			: base(Text(type)) { }

		public UMVRInvalidOperationException(Type type, Exception innerException)
			: base(Text(type), innerException) { }

		public UMVRInvalidOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		private static string Text(Type type) => string.Format(Format, type.ToPrettyString());
	}
}