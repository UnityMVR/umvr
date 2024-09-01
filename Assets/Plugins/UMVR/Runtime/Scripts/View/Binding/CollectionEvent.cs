using System;
using pindwin.umvr.Exceptions;

namespace pindwin.umvr.View.Binding
{
	public readonly struct CollectionEvent
	{
		public int Index { get; }
		public CollectionEventType Type { get; }
		public string Value { get; }

		private CollectionEvent(int index, CollectionEventType type, string value)
		{
			Index = index;
			Type = type;
			Value = value;
		}

		//todo think of a better way to avoid System.Web dependency
		public override string ToString()
		{
			
			string encodedValue = Value.Replace("%", "%25");
			encodedValue = encodedValue.Replace("|", "%7C");
			return $"{Index}|{(int)Type}|{encodedValue}";
		}

		public static CollectionEvent FromString(string collectionEvent)
		{
			try
			{
				string[] elements = collectionEvent.Split("|");

				int index = int.Parse(elements[0]);
				CollectionEventType eventType = (CollectionEventType)int.Parse(elements[1]);
				string value = elements[2].Replace("%7C", "|");
				value = value.Replace("%25", "%");
				return new CollectionEvent(index, eventType, value);
			}
			catch (Exception e)
			{
				throw new UMVRParsingException(collectionEvent, typeof(CollectionEvent), e);
			}
		}

		public static CollectionEvent Add(int index, string value) =>
			new (index, CollectionEventType.Add, value);
		
		public static CollectionEvent Add(string value) =>
			new (-1, CollectionEventType.Add, value);

		public static CollectionEvent Remove(int index) =>
			new (index, CollectionEventType.Remove, string.Empty);

		public static CollectionEvent Replace(int index, string value) =>
			new (index, CollectionEventType.Replace, value);

		public static CollectionEvent Clear => new (0, CollectionEventType.Clear, string.Empty);
	}
}