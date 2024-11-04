﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using pindwin.umvr.Editor.CodeGeneration.Designer;
using pindwin.umvr.Editor.CodeGeneration.View.AssistedTextFields;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.View.Parameters
{
	public class UnconstrainedParamWidget : BindableElement, INotifyValueChanged<DesignerSignature>
	{
		private readonly TextField _nameField;
		private readonly AssistedTextFieldWidget _typeField;

		private static readonly List<Type> _typeLookup;

		private volatile int _lastTypeLength;
		private static readonly Regex _regex = new(@"[+<>]");
		
		private DesignerSignature _value;
		
		static UnconstrainedParamWidget()
		{
			_typeLookup = new List<Type>();
		
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsGenericType || type.IsNotPublic || (type.IsSealed && type.IsAbstract))
					{
						continue;
					}

					if (type.FullName == null || _regex.IsMatch(type.FullName))
					{
						continue;
					}

					_typeLookup.Add(type);
				}
			}
		}

		public UnconstrainedParamWidget()
		{
			if (DesignerUtility.DesignerViewResources.UnconstrainedParameter == null)
			{
				return;
			}
			
			DesignerUtility.DesignerViewResources.UnconstrainedParameter.CloneTree(this);

			_typeField = this.Q<AssistedTextFieldWidget>("up-type-field");
			_typeField.Options = new List<string>();
			_typeField.RegisterValueChangedCallback(OnTypeChanged);
			
			_nameField = this.Q<TextField>("up-name-field");
			_nameField.RegisterValueChangedCallback(evt => value = new DesignerSignature(evt.newValue, ParamType));
		}

		private void OnTypeChanged(ChangeEvent<string> evt)
		{
			value = new DesignerSignature(ParamName, evt.newValue);
			_typeField.Options.Clear();
			
			if (evt.newValue.Length >= _typeField.GracePeriodLength)
			{
				foreach (Type type in _typeLookup)
				{
					if (type.Name.Contains(evt.newValue, StringComparison.InvariantCultureIgnoreCase))
					{
						_typeField.Options.Add(type.FullName);
					}
				}
			}
			
			_typeField.RefreshListDisplay();
		}

		public void SetValueWithoutNotify(DesignerSignature newValue)
		{
			_value = newValue;
			
			_nameField.SetValueWithoutNotify(newValue?.Name ?? string.Empty);
			_typeField.SetValueWithoutNotify(newValue?.Type ?? string.Empty);
		}

		public DesignerSignature value
		{
			get => _value;
			set
			{
				using var pooled = ChangeEvent<DesignerSignature>.GetPooled(this.value, value);
				
				pooled.target = this;
				SetValueWithoutNotify(value);
				SendEvent(pooled);
			}
		}
		
		public string ParamName
		{
			get => _nameField.value;
			set
			{
				if (_value == null)
				{
					_value = new DesignerSignature(value, string.Empty);
				}
				
				_value.Name = value;
				_nameField.value = value;
			}
		}

		public string ParamType
		{
			get => _typeField.TextValue;
			set
			{
				if (_value == null)
				{
					_value = new DesignerSignature(string.Empty, value);
				}
				
				_value.Type = value;
				_typeField.TextValue = value;
			} 
		}
		
		public new class UxmlFactory : UxmlFactory<UnconstrainedParamWidget, UxmlTraits> { }
		
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			private readonly UxmlStringAttributeDescription _upName = new() { name = "param-name" };
			private readonly UxmlStringAttributeDescription _upType = new() { name = "param-type" };

			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				 if (ve is not UnconstrainedParamWidget param)
				 {
				 	return;
				 }

				param.ParamName = _upName.GetValueFromBag(bag, cc);
				param.ParamType = _upType.GetValueFromBag(bag, cc);
			}
		}
	}
}