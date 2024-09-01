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
				Property prop2 = new Property();
				if (p.GetMethod == null)
				{
					logger.Log($"{typeof(GenConcreteModel)} ignored {type.ToPrettyString()}.{p.Name} write-only property when generating model.{Environment.NewLine}" +
							   $"Substitute it with a method or add public getter along with {typeof(CustomImplementationAttribute)}, if you want a simple, non-reactive data-storage.{Environment.NewLine}" +
							   "Add get method, if you want it to become a typical, reactive property.", LogSeverity.Error);
					continue;
				}

				prop2.Name = p.Name;
				prop2.IsCollection = p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>);
				Type propertyType = prop2.IsCollection ? p.PropertyType.GenericTypeArguments[0] : p.PropertyType;
				prop2.Type = propertyType.FullName;

				prop2.IsReadonly = p.SetMethod == null;
				var initialization = (InitializationAttribute)Attribute.GetCustomAttribute(p, typeof(InitializationAttribute));
				if (prop2.IsReadonly)
				{
					if (initialization != null && initialization.Level != InitializationLevel.Explicit)
					{
						logger.Log($"Ignoring value of {type.ToPrettyString()}.{p.Name} {typeof(InitializationAttribute)}.{Environment.NewLine}" +
								   $"Since property has no setter, it's considered readonly and defaults to explicit initialization.{Environment.NewLine}" +
								   $"If you want to use specified initialization level, add a set method.", LogSeverity.Warning);
					}

					prop2.InitializationLevel = InitializationLevel.Explicit;
				}
				else
				{
					prop2.InitializationLevel = initialization?.Level ?? InitializationLevel.Default;
				}

				if (prop2.InitializationLevel == InitializationLevel.Explicit)
				{
					Constructors[0].Params.Add(new Parameter(prop2.Type, prop2.FieldName.Substring(1)));
				}

				prop2.IsModel = typeof(IModel).IsAssignableFrom(propertyType);
				prop2.CustomImplementation = HasAttribute(p, typeof(CustomImplementationAttribute));
				prop2.IsCommand = typeof(ICommand).IsAssignableFrom(propertyType);

				var cascade = (CascadeDisposeAttribute) Attribute.GetCustomAttribute(
					p,
					typeof(CascadeDisposeAttribute)
				);
				prop2.CascadeDirection = cascade?.Direction ?? CascadeDirection.None;

				Properties.Add(prop2);
			}
			
			TryAddAdditionalParameters(type);
			TryAddMethods(type);
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
			if (methods?.Count > 0)
			{
				Methods.AddRange(methods);
			}
		}
	}
}