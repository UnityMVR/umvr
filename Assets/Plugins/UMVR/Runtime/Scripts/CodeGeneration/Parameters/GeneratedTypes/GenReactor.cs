using GenerationParams.Utilities;

namespace GenerationParams
{
	public class GenReactor : GenType
	{
		public const string Format = "{0}Reactor";

		public GenReactor(GenConcreteModel model)
			: base(model.Name, Format, model.Namespace, new string[] { "public", "partial", "class" })
		{
			BaseTypes.Add(new Parameter($"Reactor<{model.Type}>"));
			
			Constructors.Add(new Constructor(Type, new []{"public"}));
			Constructors[0].Params.Add(new Parameter(model.Type, model.Type.GetParamName()));
			Constructors[0].BaseConstructor = new Constructor(BaseTypes[0].Type);
			Constructors[0].BaseConstructor.Params.Add(Constructors[0].Params[0]);
		}
	}
}