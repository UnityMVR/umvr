using System.Reflection;
using pindwin.development;

namespace GenerationParams
{
	public class Indexer : Member
	{
		public SimpleTokensCollection Descriptors { get; }
		public ParametersCollection Params { get; protected set; }
		
		private string Accessors { get; }
		
		public Indexer(PropertyInfo info)
		{
			Type = info.PropertyType.ToPrettyString();
			Name = "this";
			
			Descriptors = new SimpleTokensCollection(new []{"public"});
			
			Params = new ParametersCollection();
			foreach (ParameterInfo indexParameter in info.GetIndexParameters())
			{
				Params.Add(new Parameter(indexParameter.ParameterType.ToPrettyString(), indexParameter.Name));
			}

			Accessors = $"{(info.GetMethod != null ? "get => throw new System.NotImplementedException(); " : "")} {(info.SetMethod != null ? "set => throw new System.NotImplementedException(); " : "")}";
		}
		
		public string ResolveSignature()
		{
			return $"{Descriptors.ToCollectionString()} {Type} {Name}[{Params.ToMethodSignatureString()}] {{ {Accessors}}}";
		}
	}
}