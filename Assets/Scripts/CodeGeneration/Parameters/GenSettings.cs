using System;
using System.Collections.Generic;
using pindwin.development;

namespace GenerationParams
{
	public class GenSettings
	{
		public string Name { get; }
		public string Namespace { get; }

		public GenConcreteModel GenConcreteModel { get; }
		public GenConcreteFactory GenConcreteFactory { get; }
		public GenReactor GenReactor { get; }
		public GenReactorFactory GenReactorFactory { get; }
		public GenRepository GenRepository { get; }

		public Dictionary<string, string> Vars = new Dictionary<string, string>();

		public GenSettings(string name, string @namespace, Type interfaceType, IDictionary<string, ILogger> loggers)
		{
			Name = name;
			Namespace = @namespace;
			
			GenConcreteModel = new GenConcreteModel(@namespace, interfaceType, loggers.TryGetValue("Model", out var logger) ? logger : new NullLogger());
			GenConcreteFactory = new GenConcreteFactory(GenConcreteModel);
			GenReactor = new GenReactor(GenConcreteModel);
			GenReactorFactory = new GenReactorFactory(GenConcreteModel);
			GenRepository = new GenRepository(GenConcreteModel);
		}
	}
}