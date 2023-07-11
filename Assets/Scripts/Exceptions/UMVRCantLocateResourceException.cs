using System;
using System.Runtime.Serialization;

namespace pindwin.umvr.Exceptions
{
	public class UMVRCantLocateResourceException : UMVRException
	{
		private const string FORMAT = "Cannot locate resource {0} in {1}";

		private static string Text(Type expectedType, Type containerType) =>
			string.Format(FORMAT, expectedType, containerType);
		
		public UMVRCantLocateResourceException(Type expectedType, Type containerType)
			: base(Text(expectedType, containerType)) { }

		public UMVRCantLocateResourceException(Type expectedType, Type containerType, Exception innerException)
			: base(Text(expectedType, containerType), innerException) { }

		public UMVRCantLocateResourceException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}