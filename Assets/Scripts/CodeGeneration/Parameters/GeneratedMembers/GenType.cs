using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GenerationParams
{
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
	public abstract class GenType : Token
	{
		public string Namespace { get; }
		public sealed override string Name { get; }
		public string Type { get; }
		public SimpleTokensCollection Descriptors { get; }
		public TypeParametersCollection GenericParameters { get; } = new();
		public TypeParametersCollection InstallationGenericParameters { get; } = new();
		public TypeParametersCollection BaseTypes { get; } = new();
		public ConstraintsCollection GenericTypeConstraints { get; } = new();
		public TokensCollection<Property> Properties { get; } = new();
		public TokensCollection<Constructor> Constructors { get; } = new();
		public TokensCollection<Method> Methods { get; } = new();

		public string ToDeclarationString(string currentIndent, string indent = "	")
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"{Descriptors.ToCollectionString()} {Type}");
			if (GenericParameters.Count > 0)
			{
				sb.Append($"<{GenericParameters.ToTypeParametersString()}>");
			}

			if (BaseTypes.Count > 0)
			{
				sb.Append($" : {BaseTypes.ToTypeParametersString()}");
			}
			
			if (GenericTypeConstraints.Count > 0)
			{
				currentIndent = $"{Environment.NewLine}{currentIndent}{indent}";
				sb.Append(currentIndent);
				sb.Append($"{GenericTypeConstraints.ToConstraintsString($"{Environment.NewLine}{currentIndent}")}");
			}

			return sb.ToString();
		}

		public string ToPartialDeclarationString()
		{
			return $"{Descriptors.ToCollectionString()} {Type}";
		}

		private GenType(string name, string typeFormat, string @namespace)
		{
			Name = name;
			Type = string.Format(typeFormat, Name);
			Namespace = @namespace;
		}

		protected GenType(string name, string typeFormat, string @namespace, IEnumerable<string> accessModifiers)
			: this(name, typeFormat, @namespace)
		{
			Descriptors = new SimpleTokensCollection(accessModifiers);
		}
	}
}