using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pindwin.umvr.Attributes;
using pindwin.umvr.Editor.Extensions;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;

namespace GenerationParams
{
	public class Model
	{
		public Type UnderlyingType { get; set; }
		public string Name { get; set; }
		public bool IsSingleton { get; set; }
		public List<Parameter> AdditionalParameters { get; set; }
		public List<Property> Properties { get; set; }
		public List<Method> Methods { get; set; } = new();
		public List<Event> Events { get; set; } = new();
		public List<Indexer> Indexers { get; set; } = new();

		public Model(Type type)
		{
			AssertArgument(type);
			
			UnderlyingType = type;
			Name = type.MakeClassName();
			IsSingleton = type.GetCustomAttribute<SingletonAttribute>() != null;
			
			Properties = new List<Property>();
			foreach (PropertyInfo p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				if (p.GetIndexParameters().Length > 0)
				{
					Indexers.Add(new Indexer(p));
					continue;
				}
				
				Properties.Add(new Property(p));
			}
			
			int count = default;
			AdditionalParameters = Attribute.GetCustomAttributes(type, typeof(AdditionalParameterAttribute)).
				Cast<AdditionalParameterAttribute>().
				Select(p => new Parameter(p.Type.FullName, p.Name ?? $"param{count++}"))
				.ToList();
			
			Methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).
				Select(m => new Method(m)).
				ToList();
			
			Events = type.GetEvents(BindingFlags.Public | BindingFlags.Instance).
				Select(e => new Event(e)).
				ToList();
		}

		private static void AssertArgument(Type interfaceType)
		{
			if (!typeof(IModel).IsAssignableFrom(interfaceType) || !interfaceType.IsInterface)
			{
				throw new UMVRException("Provided type is not an interface or does not inherit from IModel.");
			}

			if (!interfaceType.Name.StartsWith("I"))
			{
				throw new UMVRException("Provided type does not follow the naming convention I{Name}.");
			}
		}
	}
}