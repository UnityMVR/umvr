using System;
using pindwin.umvr.Command;
using pindwin.umvr.Exceptions;

namespace pindwin.umvr.View.Parsing
{
	public abstract class PropertyParser : IValidatable<string>
	{
		public abstract Type Type { get; }

		public void Parse<TProperty>(string value, TProperty property)
		{
			if (TryParse(value, property))
			{
				return;
			}
			
			throw new UMVRParsingException(value, Type);
		}

		protected abstract bool TryParse<TProperty>(string value, TProperty property);
		public abstract bool IsValid(string payload);
	}
}