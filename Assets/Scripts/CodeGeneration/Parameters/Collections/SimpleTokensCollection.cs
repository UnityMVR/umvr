using System.Collections.Generic;
using System.Linq;

namespace GenerationParams
{
	public class SimpleTokensCollection : TokensCollection<Token>
	{
		private readonly string _delimiter;
		
		public SimpleTokensCollection(IEnumerable<string> accessModifiers, string delimiter = " ")
		{
			_delimiter = delimiter;
			Members.AddRange(accessModifiers.Select(s => (Token)s));
		}
		
		public string ToCollectionString()
		{
			return string.Join(_delimiter, Members);
		}
	}
}