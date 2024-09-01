using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using pindwin.development;

namespace GenerationParams
{
	public class Method : Member
	{
		public SimpleTokensCollection Descriptors { get; }
		public TypeParametersCollection GenericParams { get; protected set; }
		public ConstraintsCollection GenericTypeConstraints { get; protected set; }
		public ParametersCollection Params { get; protected set; }

		public string ResolveSignature()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"public {Type} {Name}");
			if (GenericParams.Count > 0)
			{
				sb.Append($"<{GenericParams.ToTypeParametersString()}>");
			}
			sb.Append($"({Params.ToMethodSignatureString()})");
			if (GenericTypeConstraints.Count > 0)
			{
				sb.Append(" ");
				sb.Append(GenericTypeConstraints.ToConstraintsString());
			}
			return sb.ToString();
		}

		public Method(MethodInfo methodInfo) : 
			this(methodInfo.ReturnType != typeof(void) ? methodInfo.ReturnType.ToPrettyString() : "void", 
				 GetMethodDescriptors(methodInfo))
		{
			Name = methodInfo.Name;
			Params = new ParametersCollection();
			GenericParams = new TypeParametersCollection();
			GenericTypeConstraints = new ConstraintsCollection();
			
			Params.AddRange(methodInfo.GetParameters().Select(p => new Parameter(p.ParameterType.ToPrettyString(), p.Name)));
			GenericParams.AddRange(methodInfo.GetGenericArguments().Select(t => new Parameter(t.Name, string.Empty)));
			if (methodInfo.IsGenericMethod == false)
			{
				return;
			}
			
			foreach (Type genericArgument in methodInfo.GetGenericMethodDefinition().GetGenericArguments())
			{
				TypeParametersCollection collection = new TypeParametersCollection();
				collection.AddRange(genericArgument.GetGenericParameterConstraints().Select(t => new Parameter(t.Name, t.Name)));
				if (collection.Count > 0)
				{
					GenericTypeConstraints.Add(new Constraint(genericArgument.Name, collection));
				}
			}
		}

		public Method(string type, IEnumerable<string> descriptors)
		{
			Type = type;
			Params = new ParametersCollection();
			Descriptors = new SimpleTokensCollection(descriptors);
		}

		private static IEnumerable<string> GetMethodDescriptors(MethodInfo info)
		{
			List<string> result = new List<string>();
			//todo fix for internal
			if (info.IsPublic)
			{
				result.Add("public");
			}
			else if(info.IsFamily)
			{
				result.Add("protected");
			}
			else
			{
				result.Add("private");
			}

			if (info.IsAbstract)
			{
				result.Add("abstract");
			}
			else if (info.IsVirtual)
			{
				result.Add("virtual");
			}

			if (info.IsFinal)
			{
				result.Add("sealed");
			}

			if (info.IsHideBySig)
			{
				result.Add("new");
			}

			return result;
		}
	}
}