using System;
using System.Runtime.Serialization;

namespace pindwin.umvr.Exceptions
{
	public class UMVRParsingException : UMVRException
	{
		private const string ParsingErrorFormat = "Failed to parse {0} to type {1}";

		private static string Text(string payload, Type targetType) =>
			string.Format(ParsingErrorFormat, payload, targetType);
		
		public UMVRParsingException(string payload, Type targetType)
			: base(Text(payload, targetType)) { }

		public UMVRParsingException(string payload, Type targetType, Exception innerException)
			: base(Text(payload, targetType), innerException) { }

		public UMVRParsingException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}