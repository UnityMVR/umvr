using System;
using pindwin.umvr.Attributes;
using pindwin.umvr.Model;

// ReSharper disable once CheckNamespace
namespace GenerationParams
{
	public class GenConcreteModel : GenType
	{
		public const string Format = "{0}Model";
		
		public readonly ParametersCollection AdditionalParameters = new ();
		public Type UnderlyingInterfaceType { get; }

		public GenConcreteModel(string @namespace, Model model) 
			: base(model.Name, Format, @namespace, new [] { "public", "partial", "class" })
		{
			UnderlyingInterfaceType = model.UnderlyingType;
			BaseTypes.AddRange(new []{new Parameter($"Model<{Type}>"), new Parameter($"I{Name}")});
			
			Constructors.Add(new Constructor(Type, new []{ "public" }));
			Constructors[0].Params.Add(new Parameter(typeof(Id)));
			Constructors[0].BaseConstructor = new Constructor(BaseTypes[0].Type);
			Constructors[0].BaseConstructor.Params.Add(new Parameter(typeof(Id)));

			foreach (Property prop in model.Properties)
			{
				if (prop.InitializationLevel == InitializationLevel.Explicit)
				{
					Constructors[0].Params.Add(
						new Parameter(prop.IsCollection 
										  ? $"IEnumerable<{prop.Type}>" 
										  : prop.Type, prop.FieldName.Substring(1)));
				}
				
				Properties.Add(prop);
			}
			
			foreach (Parameter parameter in model.AdditionalParameters)
			{
				Constructors[0].Params.Add(parameter);
				AdditionalParameters.Add(parameter);
			}
			
			Events.AddRange(model.Events);
			Indexers.AddRange(model.Indexers);
		}
	}
}