using System;
using pindwin.umvr.Model;

namespace pindwin.umvr.View.Parsing
{
	public sealed class IntPropertyParser : PropertyParser
	{
		public override Type Type => typeof(int);
		protected override bool TryParse<TProperty>(string value, TProperty property)
		{
			if (int.TryParse(value, out int result) && property is IValueContainer<int> intProperty)
			{
				intProperty.Value = result;
				return true;
			}

			return false;
		}

		public override bool IsValid(string payload)
		{
			return int.TryParse(payload, out int _);
		}
	}
}