using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using pindwin.development;
using pindwin.umvr.Attributes;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;

namespace GenerationParams
{
	public class Property : Member
	{
		public bool IsReadonly { get; set; }
		public bool IsCollection { get; set; }
		public bool IsModel { get; set; }
		public bool CustomImplementation { get; set; }
		public bool InitializeExplicitly => InitializationLevel == InitializationLevel.Explicit;
		public bool DoNotInitialize => InitializationLevel == InitializationLevel.Skip;
		public bool CascadeDisposeUpstream => (CascadeDirection & CascadeDirection.Upstream) == CascadeDirection.Upstream;
		public bool CascadeDisposeDownstream => (CascadeDirection & CascadeDirection.Downstream) == CascadeDirection.Downstream;
		public InitializationLevel InitializationLevel { get; set; } 
		public CascadeDirection CascadeDirection { get; set; }
		public bool IsGenericProperty { get; set; } 
		
		public string Signature
		{
			get
			{
				if (IsCollection)
				{
					return $"public {nameof(IList)}<{Type}> {Name}";
				}
				
				return $"public {Type} {Name}";
			}
		}

		public string FieldName => $"_{char.ToLower(Name[0])}{Name.Substring(1)}";

		public Property()
		{ }

		public Property(PropertyInfo info)
		{
			if (info.GetMethod == null)
			{
				throw new UMVRException($"Write-only properties are not supported ({info.Name}).");
			}
			
			Name = info.Name;
			IsCollection = info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(IList<>);
			Type propType = IsCollection ? info.PropertyType.GenericTypeArguments[0] : info.PropertyType;
			Type = propType.ToPrettyString();
			CustomImplementation = HasAttribute(info, typeof(CustomImplementationAttribute));
			IsReadonly = info.SetMethod == null && !IsCollection;
			InitializationLevel = HasAttribute(info, typeof(InitializationAttribute)) 
				? ((InitializationAttribute)Attribute.GetCustomAttribute(info, typeof(InitializationAttribute))).Level 
				: InitializationLevel.Explicit;
			
			IsModel = typeof(IModel).IsAssignableFrom(propType);
			CascadeDirection = HasAttribute(info, typeof(CascadeDisposeAttribute)) 
				? ((CascadeDisposeAttribute)Attribute.GetCustomAttribute(info, typeof(CascadeDisposeAttribute))).Direction 
				: CascadeDirection.None;
			IsGenericProperty = HasAttribute(info, typeof(GenericPropertyAttribute)) && info.PropertyType.IsGenericType;
		}
	}
}