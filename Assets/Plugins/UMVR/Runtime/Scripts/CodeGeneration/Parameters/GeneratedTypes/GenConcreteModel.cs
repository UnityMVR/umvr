using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pindwin.development;
using pindwin.umvr.Attributes;
using pindwin.umvr.Command;
using pindwin.umvr.Model;

namespace GenerationParams
{
	public class GenConcreteModel : GenType
	{
		public const string Format = "{0}Model";
		
		public ParametersCollection AdditionalParameters = new ParametersCollection();
		public Type UnderlyingInterfaceType { get; }
		
		public GenConcreteModel(string @namespace, Type type, ILogger logger) 
			: base($"{type.Name.Substring(1)}", Format, @namespace, new string[] { "public", "partial", "class" })
		{
			UnderlyingInterfaceType = type;
			BaseTypes.AddRange(new []{new Parameter($"Model<{Type}>"), new Parameter($"I{Name}")});
			
			Constructors.Add(new Constructor(Type, new string[]{ "public" }));
			Constructors[0].Params.Add(new Parameter(typeof(Id)));
			Constructors[0].BaseConstructor = new Constructor(BaseTypes[0].Type);
			Constructors[0].BaseConstructor.Params.Add(new Parameter(typeof(Id)));
			foreach (PropertyInfo p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				if ((p.GetIndexParameters()?.Length ?? 0) > 0)
				{
					Indexers.Add(new Indexer(p));
					continue;
				}
				
				Property prop = new Property();
				if (p.GetMethod == null)
				{
					logger.Log($"{typeof(GenConcreteModel)} ignored {type.ToPrettyString()}.{p.Name} write-only property when generating model.{Environment.NewLine}" +
							   $"Substitute it with a method or add public getter along with {typeof(CustomImplementationAttribute)}, if you want a simple, non-reactive data-storage.{Environment.NewLine}" +
							   "Add get method, if you want it to become a typical, reactive property.", LogSeverity.Error);
					continue;
				}

				prop.Name = p.Name;
				prop.IsCollection = p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>);
				Type propertyType = prop.IsCollection ? p.PropertyType.GenericTypeArguments[0] : p.PropertyType;
				prop.Type = propertyType.ToPrettyString();
				prop.CustomImplementation = HasAttribute(p, typeof(CustomImplementationAttribute));
				prop.IsReadonly = p.SetMethod == null && !prop.IsCollection;
				
				var initialization = (InitializationAttribute)Attribute.GetCustomAttribute(p, typeof(InitializationAttribute));
				if (prop.IsReadonly && prop.CustomImplementation == false)
				{
					if (initialization != null && initialization.Level != InitializationLevel.Explicit)
					{
						logger.Log($"Ignoring value of {type.ToPrettyString()}.{p.Name} {typeof(InitializationAttribute)}.{Environment.NewLine}" +
								   $"Since property has no setter or [CustomImplementation], it's considered readonly and defaults to explicit initialization.{Environment.NewLine}" +
								   $"If you want to implement it manually, add [CustomImplementation].{Environment.NewLine}" +
								   $"If you want to use specified initialization level, add a set method.", LogSeverity.Warning);
					}

					prop.InitializationLevel = InitializationLevel.Explicit;
				}
				else
				{
					prop.InitializationLevel = initialization?.Level ?? InitializationLevel.Default;
				}

				if (prop.InitializationLevel == InitializationLevel.Explicit)
				{
					Constructors[0].Params.Add(new Parameter(prop.IsCollection ? $"IEnumerable<{prop.Type}>" : prop.Type, prop.FieldName.Substring(1)));
				}

				prop.IsModel = typeof(IModel).IsAssignableFrom(propertyType);
				prop.IsCommand = typeof(ICommand).IsAssignableFrom(propertyType);

				var cascade = (CascadeDisposeAttribute) Attribute.GetCustomAttribute(
					p,
					typeof(CascadeDisposeAttribute)
				);
				prop.CascadeDirection = cascade?.Direction ?? CascadeDirection.None;
				if (prop.CascadeDirection != CascadeDirection.None && !prop.IsModel)
				{
					logger.Log($"Ignoring value of {type.ToPrettyString()}.{p.Name} {typeof(CascadeDisposeAttribute)}.{Environment.NewLine}" +
							   $"Property is not an IModel, so it's not possible to cascade dispose it.", LogSeverity.Warning);
					
					prop.CascadeDirection = CascadeDirection.None;
				}

				Properties.Add(prop);
			}
			
			TryAddAdditionalParameters(type);
			TryAddMethods(type);
			TryAddEvents(type);
		}

		private void TryAddAdditionalParameters(MemberInfo type)
		{
			int count = default;
			List<Parameter> parameters = ((AdditionalParametersAttribute) Attribute.GetCustomAttribute(
					type,
					typeof(AdditionalParametersAttribute)
				))?.Types.Select(p => new Parameter(p.FullName, $"param{count++}")).ToList();
			if (parameters == null)
			{
				return;
			}
			
			foreach (Parameter parameter in parameters)
			{
				AdditionalParameters.Add(parameter);
				Constructors[0].Params.Add(parameter);
			}
		}

		private void TryAddMethods(Type type)
		{
			List<Method> methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
				.Where(m => m.IsSpecialName == false)
				.Select(m => new Method(m)).ToList();
			
			if (methods.Count > 0)
			{
				Methods.AddRange(methods);
			}
		}
		
		private void TryAddEvents(Type type)
		{
			List<Event> events = type.GetEvents(BindingFlags.Public | BindingFlags.Instance).Where(e => e.IsSpecialName == false)
				.Select(e => new Event(e)).ToList();
			
			if (events.Count > 0)
			{
				Events.AddRange(events);
			}
		}
	}
}