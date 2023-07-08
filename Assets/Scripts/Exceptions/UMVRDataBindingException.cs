using System;
using System.Runtime.Serialization;
using pindwin.development;

namespace pindwin.umvr.Exceptions
{
	public class UMVRDataBindingException : UMVRException
	{
		private const string Format = "Failed to obtain data stream {0} {1} in {2}";
		private static string Text(Type property, string name, Type member)
		{
			return string.Format(Format, property.ToPrettyString(), name, member.ToPrettyString());
		}

		public UMVRDataBindingException(Type dataType, string streamName, Type memberType)
			: base(Text(dataType, streamName, memberType)) { }

		public UMVRDataBindingException(Type dataType, string streamName, Type memberType, Exception innerException)
			: base(Text(dataType, streamName, memberType), innerException) { }

		public UMVRDataBindingException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
	}
}