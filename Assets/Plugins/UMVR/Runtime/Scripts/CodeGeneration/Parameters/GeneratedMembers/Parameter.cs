using System;
using GenerationParams.Utilities;
using pindwin.development;

namespace GenerationParams
{
	public class Parameter : Token
	{
		public Parameter(string type, string name)
		{
			_typeToken = type;
			_nameToken = name;
			
			Attributes = new ParametersCollection();
		}

		public Parameter(Type type)
			: this(type.ToPrettyString(), type.Name.GetParamName()) { }

		public Parameter(Type t, string name)
			: this(t.ToPrettyString(), name) { }

		public Parameter(string type) : this(type, string.Empty)
		{ }

		public string Type => _typeToken.Name;
		public override string Name => _nameToken.Name;

		private readonly Token _typeToken;
		private readonly Token _nameToken;
		
		public ParametersCollection Attributes { get; }

		public string ToSignatureString()
		{
			if (Attributes.Count > 0)
			{
				return $"[{Attributes.ToTypeParametersString()}] {Type} {Name}";
			}
			
			return $"{Type} {Name}";
		}
	}
}