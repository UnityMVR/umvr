using System;
using pindwin.umvr.Model;

namespace pindwin.umvr.View.Parsing
{
	public sealed class BoolPropertyParser : PropertyParser
	{
		public override Type Type => typeof(bool);
		protected override bool TryParse<TProperty>(string value, TProperty property)
		{
			if (bool.TryParse(value, out bool result) && property is IValueContainer<bool> boolProperty)
			{
				boolProperty.Value = result;
				return true;
			}

			return false;
		}

		public override bool IsValid(string payload)
		{
			return bool.TryParse(payload, out bool _);
		}
	}
}