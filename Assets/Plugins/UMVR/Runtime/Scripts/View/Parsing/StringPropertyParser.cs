using System;
using pindwin.umvr.Model;

namespace pindwin.umvr.View.Parsing
{
	public sealed class StringPropertyParser : PropertyParser
	{
		public override Type Type => typeof(string);
		protected override bool TryParse<TProperty>(string value, TProperty property)
		{
			if (property is IValueContainer<string> stringProperty)
			{
				stringProperty.Value = value;
				return true;
			}

			return false;
		}

		public override bool IsValid(string payload)
		{
			return true;
		}
	}
}