using System;
using System.Linq;
using System.Reflection;

namespace GenerationParams
{
	public class Token
	{
		public virtual string Name { get; }
		public Token(string name)
		{
			Name = name;
		}

		protected Token()
		{ }
		
		protected static bool HasAttribute(MemberInfo decoratedType, Type attributeType)
		{
			return decoratedType.CustomAttributes.Any(c => c.AttributeType == attributeType);
		}

		public static implicit operator Token(string s) => new Token(s);
		public static implicit operator string(Token t) => t.Name;

		public override string ToString()
		{
			return Name;
		}
	}
}