using System;
using System.Linq;

namespace GenerationParams
{
	public class ConstraintsCollection : TokensCollection<Constraint>
	{
		public string ToConstraintsString(string delimiter = " ")
		{
			return string.Join(delimiter, Members.Select(m => m.ToSingleConstraintString()));
		}
	}
}