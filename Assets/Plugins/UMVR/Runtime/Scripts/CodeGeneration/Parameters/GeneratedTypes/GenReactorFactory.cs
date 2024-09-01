namespace GenerationParams
{
	public class GenReactorFactory : GenType
	{
		public const string Format = "{0}ReactorFactory";

		public GenReactorFactory(GenConcreteModel model)
			: base(model.Name, Format, model.Namespace, new string[] { "public", "class" })
		{
			BaseTypes.Add(new Parameter($"ReactorFactory<{string.Format(GenConcreteModel.Format, Name)}, {string.Format(GenReactor.Format, Name)}>"));
		}
	}
}