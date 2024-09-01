using System;
using System.Collections;
using System.Collections.Generic;
using pindwin.umvr.Command;
using pindwin.umvr.Model;

namespace pindwin.umvr.View.Binding
{
	public sealed class CollectionAutoBinding : IDisposable
	{
		private readonly List<object> _buffer;
		private readonly List<object> _cache;
		private readonly IBindingHandler _handler;
		private readonly Property _property;

		public CollectionAutoBinding(
			Property property,
			IBindingHandler handler,
			IConditionalCommand<string> feedbackCommand)
		{
			_property = property;
			_handler = handler;
			_cache = new List<object>();
			_buffer = new List<object>();

			if (property is IEnumerable enumerableProperty)
			{
				foreach (object o in enumerableProperty)
				{
					_cache.Add(o);
					handler.FeedToView(CollectionEvent.Add(o.ToString()));
				}
			}

			property.Changed += Refresh;
			_handler.BindFeedback(feedbackCommand);
		}

		public void Dispose()
		{
			_property.Changed -= Refresh;
		}

		private void Refresh()
		{
			_buffer.Clear();
			IEnumerable enumerable = _property as IEnumerable;
			if (enumerable == null)
			{
				return;
			}

			foreach (object o in enumerable)
			{
				_buffer.Add(o);
			}

			if (_buffer.Count == 0 && _cache.Count > 0)
			{
				_cache.Clear();
				_handler.FeedToView(CollectionEvent.Clear);
				return;
			}

			int i = 0;
			for (int count = _buffer.Count; i < count; i++)
			{
				if (_cache.Count <= i)
				{
					_cache.Add(_buffer[i]);
					_handler.FeedToView(CollectionEvent.Add(GetString(_buffer[i])));
					continue;
				}

				if (_cache[i] != _buffer[i])
				{
					_cache[i] = _buffer[i];
					_handler.FeedToView(CollectionEvent.Replace(i, GetString(_cache[i])));
				}
			}

			while (i < _cache.Count)
			{
				_handler.FeedToView(CollectionEvent.Remove(_cache.Count - 1));
				_cache.RemoveAt(_cache.Count - 1);
			}
		}

		private static string GetString(object o)
		{
			return o != null ? o.ToString() : "null";
		}
	}
}