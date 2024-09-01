using System.Linq;

namespace GenerationParams
{
	public class ParametersCollection : TokensCollection<Parameter>
	{
		public string ToMethodSignatureString()
		{
			return string.Join(", ", Members.Select(m => m.ToSignatureString()));
		}

		public string ToTypeParametersString()
		{
			return string.Join(", ", Members.Select(m => m.Type));
		}

		public string ToParameterNamesString()
		{
			return string.Join(", ", Members.Select(m => m.Name));
		}
	}
}