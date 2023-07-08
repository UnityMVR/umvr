using System;
using System.Runtime.Serialization;

namespace pindwin.umvr.Exceptions
{
	public class UMVRDataTypeMismatchException : UMVRException
	{
		private const string Format = "Data type mismatch when resolving model {0}. Expected {1}, received {2}.";

		private static string Text(Type @interface, Type expected, Type received) =>
			string.Format(Format, @interface, expected, received);
		
		public UMVRDataTypeMismatchException(Type @interface, Type expected, Type received)
			: base(Text(@interface, expected, received)) { }

		public UMVRDataTypeMismatchException(Type @interface, Type expected, Type received, Exception innerException)
			: base(Text(@interface, expected, received), innerException) { }

		public UMVRDataTypeMismatchException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}