using System.Linq;

namespace GenerationParams
{
	public class TypeParametersCollection : TokensCollection<Parameter>
	{
		public string ToTypeParametersString()
		{
			return string.Join(", ", Members.Select(m => m.Type));
		}
	}
}