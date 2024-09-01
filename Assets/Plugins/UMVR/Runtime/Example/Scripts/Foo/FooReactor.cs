using pindwin.development;
using pindwin.umvr.Model;
using pindwin.umvr.View;
using pindwin.umvr.View.Binding;
using UniRx;
using UnityEngine;
using Zenject;

namespace pindwin.umvr.example
{
	public partial class FooReactor
	{
		private readonly IView _view;
		private readonly FeedValueBackCommand.Factory _singleFeedbackFactory;
		private readonly FeedCollectionEntryBackCommand.Factory _collectionFeedbackFactory;

		[Inject]
		public FooReactor(
			FooModel model,
			IView view,
			FeedValueBackCommand.Factory singleFeedbackFactory,
			FeedCollectionEntryBackCommand.Factory collectionFeedbackFactory)
			: base(model)
		{
			_view = view.AssertNotNull();
			_singleFeedbackFactory = singleFeedbackFactory.AssertNotNull();
			_collectionFeedbackFactory = collectionFeedbackFactory.AssertNotNull();
		}

		protected override void BindDataSourceImpl(FooModel model)
		{
			foreach (string label in model)
			{
				Property property = model.GetProperty(label);
				if (property == null)
				{
					continue;
				}

				if (property is CollectionProperty collectionProperty)
				{
					_view
						.AutoBindCollection(model, collectionProperty, _collectionFeedbackFactory.Create(collectionProperty))
						.AddTo(Subscriptions);
				}
				else
				{
					_view
						.AutoBindProperty(model, property, _singleFeedbackFactory.Create(property))
						.AddTo(Subscriptions);
				}
			}

			model.GetProperty<string>(nameof(FooModel.Text)).Skip(1).Subscribe(Debug.Log);
			model.GetCollection<int>(nameof(FooModel.Collection)).ObserveReplace()
				.Subscribe(r => Debug.Log($"New value at [{r.Index}]: {r.OldValue} -> {r.NewValue}"));
			model.GetCollection<int>(nameof(FooModel.Collection)).ObserveRemove()
				.Subscribe(r => Debug.Log($"Removed at index: {r.Index}"));
		}
	}
}