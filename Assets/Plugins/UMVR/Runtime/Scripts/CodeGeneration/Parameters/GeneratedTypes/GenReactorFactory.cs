namespace GenerationParams
{
	public class GenReactorFactory : GenType
	{
		public const string Format = "{0}ReactorFactory";
		public string NullType => $"{string.Format(Format, Name)}<Null{string.Format(GenReactor.Format, Name)}>";
		public string NonNullType => $"{string.Format(Format, Name)}<{string.Format(GenReactor.Format, Name)}>";
		
		public GenReactorFactory(GenConcreteModel model)
			: base(model.Name, Format, model.Namespace, new string[] { "internal", "class" })
		{
			BaseTypes.Add(new Parameter($"ReactorFactory<{string.Format(GenConcreteModel.Format, Name)}, TReactor>"));
			
			GenericParameters.Add(new Parameter("TReactor"));
			GenericTypeConstraints.Add(new Constraint("TReactor", new TypeParametersCollection {new Parameter($"Reactor<{model.Type}>")}));
		}
	}
}