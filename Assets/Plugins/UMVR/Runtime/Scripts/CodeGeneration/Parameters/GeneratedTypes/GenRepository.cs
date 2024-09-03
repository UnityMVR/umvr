using pindwin.umvr.Attributes;
using GenerationParams.Utilities;

namespace GenerationParams
{
	public class GenRepository : GenType
	{
		public const string Format = "{0}Repository";
		public bool IsSingleton { get; }
		
		public string GetType(string reactorType)
		{
			return $"{Name}Repository<{reactorType}>";
		}
		
		public GenRepository(GenConcreteModel model)
			: base(model.Name, Format, model.Namespace, new string[] { "public", "partial", "class" })
		{
			IsSingleton = HasAttribute(model.UnderlyingInterfaceType, typeof(SingletonAttribute));
			BaseTypes.Add(new Parameter(
							  $"{(IsSingleton ? "Singleton" : "")}Repository<I{model.Name}, {model.Type}, TReactor>"));
			
			Constructors.Add(new Constructor(Type, new string[]{"public"}));
			string typeName = string.Format(GenReactorFactory.Format, Name);
			Constructors[0].Params.Add(new Parameter($"{typeName}<TReactor>", typeName.GetParamName()));
			Constructors[0].BaseConstructor = new Constructor(BaseTypes[0].Type);
			Constructors[0].BaseConstructor.Params.Add(Constructors[0].Params[0]);
			
			GenericParameters.Add(new Parameter("TReactor"));
			GenericTypeConstraints.Add(new Constraint("TReactor", new TypeParametersCollection {new Parameter($"Reactor<{model.Type}>")}));
		}
	}
}