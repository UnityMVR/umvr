using pindwin.development;

namespace GenerationParams
{
	public class GenConcreteFactory : GenType
	{
		public const string Format = "{0}Factory";
		
		public GenConcreteFactory(GenConcreteModel model)
			: base(model.Name, Format, model.Namespace, new string[] { "public", "partial", "class" })
		{
			foreach (var property in model.Properties)
			{
				if (property.InitializeExplicitly)
				{
					InstallationGenericParameters.Add(new Parameter(property.Type));
				}
			}

			foreach (Parameter param in model.AdditionalParameters)
			{
				InstallationGenericParameters.Add(new Parameter(param.Type));
			}
			
			BaseTypes.Add(
				InstallationGenericParameters.Count == 0
					? new Parameter($"ModelFactory<I{Name}, {model.Type}>") 
					: new Parameter($"ModelFactory<I{Name}, {model.Type}, {InstallationGenericParameters.ToTypeParametersString()}>"));

			Constructors.Add(new Constructor(Type, new []{"public"}));
			Constructors[0].Params.Add(new Parameter($"IRepository<I{Name}>",  "repository"));
			Constructors[0].Params.Add(new Parameter($"ISerializer<{model.Type}>",  "serializer"));
			Constructors[0].Params[1].Attributes.Add(new Parameter("InjectOptional"));
			Constructors[0].BaseConstructor = new Constructor(BaseTypes[0]);
			Constructors[0].BaseConstructor.Params.AddRange(Constructors[0].Params);
		}

		public string GetBindingGenericParametersString()
		{
			if (InstallationGenericParameters.Count == 0)
			{
				return string.Empty;
			}

			return $", {InstallationGenericParameters.ToTypeParametersString()}";
		}
	}
}