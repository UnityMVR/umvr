using pindwin.development;
using pindwin.umvr.Command;
using pindwin.umvr.Model;
using pindwin.umvr.View.Parsing;
using Zenject;

namespace pindwin.umvr.View.Binding
{
	public sealed class FeedCollectionEntryBackCommand : IConditionalCommand<string>
	{
		private readonly CollectionProperty _property;
		private readonly CollectionParserProvider _parserProvider;
		
		public FeedCollectionEntryBackCommand(CollectionProperty property, CollectionParserProvider parserProvider)
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
		
		public class Factory : PlaceholderFactory<CollectionProperty, FeedCollectionEntryBackCommand>
		{
			private readonly CollectionParserProvider _parserProvider;

			public Factory(CollectionParserProvider parserProvider)
			{
				_parserProvider = parserProvider;
			}

			public override FeedCollectionEntryBackCommand Create(CollectionProperty param)
			{
				return new FeedCollectionEntryBackCommand(param, _parserProvider);
			}
		}
	}
}