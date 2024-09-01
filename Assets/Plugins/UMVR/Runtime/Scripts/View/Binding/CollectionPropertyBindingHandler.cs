using System;
using pindwin.development;
using pindwin.umvr.Command;
using pindwin.umvr.Exceptions;
using pindwin.umvr.View.Widgets;

namespace pindwin.umvr.View.Binding
{
	public sealed class CollectionPropertyBindingHandler : IBindingHandler
	{
		private readonly CollectionWidget _displayTarget;
		private IConditionalCommand<string> _updateMethod;
		
		public CollectionPropertyBindingHandler(CollectionWidget displayTarget)
		{
			_displayTarget = displayTarget.AssertNotNull();
		}
		
		public void FeedToView<TPropertyType>(TPropertyType value)
		{
			if (value is CollectionEvent collEvent)
			{
				switch (collEvent.Type)
				{
					case CollectionEventType.Add:
						_displayTarget.Add(collEvent.Index, collEvent.Value);
						break;
					case CollectionEventType.Remove:
						_displayTarget.Remove(collEvent.Index);
						break;
					case CollectionEventType.Replace:
						_displayTarget.Replace(collEvent.Index, collEvent.Value);
						break;
					case CollectionEventType.Clear:
						_displayTarget.ClearCollection();
						break;
					default:
						throw new ArgumentException($"Unknown {typeof(CollectionEventType)}: {collEvent.Type}");
				}
				return;
			}
			
			throw new UMVRException(string.Empty, new ArgumentException($"Expected {typeof(CollectionEvent)}, received {typeof(TPropertyType)}"));
		}

		public void FeedToView(string valueString)
		{
			throw new NotSupportedException($"Passing collection to view with {nameof(FeedToView)}(string) is not " +
											$"supported. Use generic version with {nameof(CollectionEvent)} instead.");
		}

		public void FeedBackFromView(string newValue)
		{
			_updateMethod.Execute(newValue);
		}

		public void BindFeedback(IConditionalCommand<string> updateCommand)
		{
			_updateMethod = updateCommand;
			_displayTarget.IsReadonly = _updateMethod?.CanExecute() != true;
		}

		public void Validate(string payload)
		{
			CollectionEvent ev = CollectionEvent.FromString(payload);
			if (_displayTarget.GetEntry(ev.Index) is IValidatedProperty validatable)
			{
				validatable.CanCommit = IsValid(payload);
			}
		}

		public bool IsValid(string payload)
		{
			return _updateMethod?.IsValid(payload) == true;
		}
	}
}