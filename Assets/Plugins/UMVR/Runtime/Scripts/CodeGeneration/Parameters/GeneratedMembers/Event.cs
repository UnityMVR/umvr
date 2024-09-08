using System;
using System.Reflection;
using pindwin.development;

namespace GenerationParams
{
	public class Event : Member
	{
		public SimpleTokensCollection Descriptors { get; }
		
		public Event(EventInfo info)
		{
			Type handlerType = info.EventHandlerType;

			Type = handlerType.ToPrettyString();
			Name = info.Name;
			Descriptors = new SimpleTokensCollection(new string[] { "public", "event"});
		}
		
		public string ResolveSignature()
		{
			return $"{Descriptors.ToCollectionString()} {Type} {Name};";
		}
	}
}