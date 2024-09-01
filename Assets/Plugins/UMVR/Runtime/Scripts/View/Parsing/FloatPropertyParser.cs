using System;
using pindwin.umvr.Model;

namespace pindwin.umvr.View.Parsing
{
	public sealed class FloatPropertyParser : PropertyParser
	{
		public override Type Type => typeof(float);
		
		protected override bool TryParse<TProperty>(string value, TProperty property)
		{
			if (float.TryParse(value, out float result) && property is IValueContainer<float> floatProperty)
			{
				floatProperty.Value = result;
				return true;
			}

			return false;
		}

		public override bool IsValid(string payload)
		{
			return float.TryParse(payload, out float _);
		}
	}
}