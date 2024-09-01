using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace pindwin.umvr.View.Parsing
{
	public class CollectionParserProvider
	{
		private readonly Dictionary<Type, PropertyParser> _parsers = new Dictionary<Type, PropertyParser>();
        
		[Inject]
		public CollectionParserProvider(List<PropertyParser> parsers)
		{
			foreach (PropertyParser parser in parsers.Where(p => p.Type.IsGenericType && p.Type.GetGenericTypeDefinition() == typeof(IList<>)))
			{
				_parsers[parser.Type.GenericTypeArguments[0]] = parser;
			}
		}
		
		public PropertyParser GetParser(Type t)
		{
			return _parsers.TryGetValue(t, out var parser) ? parser : null;
		}
	}
}