using pindwin.development;
using pindwin.umvr.Command;
using pindwin.umvr.Model;
using pindwin.umvr.View.Parsing;
using Zenject;

namespace pindwin.umvr.View.Binding
{
	public class FeedValueBackCommand : IConditionalCommand<string>
	{
		private readonly Property _property;
		private readonly PropertyParserProvider _parserProvider;
		
		public FeedValueBackCommand(Property property, PropertyParserProvider parserProvider)
		{
			_property = property;
			_parserProvider = parserProvider.AssertNotNull();
		}
		
		public void Execute(string param)
		{
			PropertyParser parser = _parserProvider.GetParser(_property.Type);
			parser.Parse(param, _property);
		}

		public bool CanExecute()
		{
			return _property != null && _parserProvider.GetParser(_property.Type) != null;
		}

		public bool IsValid(string payload)
		{
			if (_property == null)
			{
				return false;
			}

			PropertyParser provider = _parserProvider.GetParser(_property.Type);
			return provider?.IsValid(payload) == true;
		}
		
		public class Factory : PlaceholderFactory<Property, FeedValueBackCommand>
		{
			private readonly PropertyParserProvider _parserProvider;

			public Factory(PropertyParserProvider parserProvider)
			{
				_parserProvider = parserProvider;
			}

			public override FeedValueBackCommand Create(Property param)
			{
				return new FeedValueBackCommand(param, _parserProvider);
			}
		}
	}
}