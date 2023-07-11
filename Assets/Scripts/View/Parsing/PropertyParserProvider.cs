using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace pindwin.umvr.View.Parsing
{
	public sealed class PropertyParserProvider
	{
		private readonly Dictionary<Type, PropertyParser> _parsers;

		[Inject]
		public PropertyParserProvider(List<PropertyParser> parsers)
		{
			_parsers = new Dictionary<Type, PropertyParser>();
			foreach (PropertyParser parser in parsers.Where(p => p.Type.IsGenericType == false))
			{
				RegisterParser(parser);
			}
		}
		
		public PropertyParser GetParser(Type t)
		{
			return _parsers.TryGetValue(t, out var parser) ? parser : null;
		}

		private void RegisterParser(PropertyParser parser)
		{
			_parsers[parser.Type] = parser;
		}
	}
}