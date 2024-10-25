using System;
using System.Collections.Generic;
using pindwin.umvr.Editor.CodeGeneration.Designer;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[assembly: UxmlNamespacePrefix("pindwin.umvr.Editor.CodeGeneration.View", "view")]

namespace pindwin.umvr.Editor.CodeGeneration.View.Parameters
{
	public class ParamWidget : BindableElement, INotifyValueChanged<DesignerParameter>
	{
		private readonly TextField _nameField;
		private readonly DropdownField _typeField;

		public ParamWidget()
		{
			ParamType = nameof(Int32);
			ParamName = string.Empty;
			
			style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
			style.flexGrow = new StyleFloat(1.0f);
			style.height = new StyleLength(20.0f);
			style.maxHeight = new StyleLength(20.0f);

			_typeField = new DropdownField(string.Empty)
			{
				name = "param-type-dropdown",
				choices = new List<string>(DesignerUtility.LegalTypeNames),
				style =
				{
					flexGrow = 1.0f,
					maxWidth = new StyleLength(new Length(50.0f, LengthUnit.Percent))
				}
			};
			_typeField.RegisterValueChangedCallback(evt => value = new DesignerParameter(ParamName, evt.newValue));
			
			Add(_typeField);

			_nameField = new TextField(string.Empty)
			{
				name = "param-name-textfield",
				style =
				{
					flexGrow = 1.0f,
					maxWidth = new StyleLength(new Length(50.0f, LengthUnit.Percent))
				}
			};
			_nameField.RegisterValueChangedCallback(evt => value = new DesignerParameter(evt.newValue, ParamType));
			
			Add(_nameField);
		}

		public string ParamName
		{
			get => _nameField.value;
			set
			{
				if (_nameField != null)
				{
					_nameField.value = value;
				}
			}
		}

		public string ParamType
		{
			get => _typeField.value;
			set
			{
				if (_typeField != null)
				{
					_typeField.value = value;
				}
			}
		}

		public DesignerParameter value
		{
			get => new (ParamName, ParamType);
			set
			{
				using ChangeEvent<DesignerParameter> pooled = ChangeEvent<DesignerParameter>.GetPooled(new DesignerParameter(ParamName, ParamType), value);

				pooled.target = this;
				SetValueWithoutNotify(value);
				SendEvent(pooled);
			}
		}

		public void SetValueWithoutNotify(DesignerParameter newValue)
		{
			ParamName = newValue?.Name ?? string.Empty;
			ParamType = newValue?.Type ?? string.Empty;
			
			MarkDirtyRepaint();
		}

		public new class UxmlFactory : UxmlFactory<ParamWidget, UxmlTraits> { }

		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			private readonly UxmlStringAttributeDescription _paramName = new() { name = "param-name" };
			private readonly UxmlStringAttributeDescription _paramType = new() { name = "param-type" };

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

				if (ve is not ParamWidget param)
				{
					return;
				}

				param.ParamType = _paramType.GetValueFromBag(bag, cc);
				param.ParamName = _paramName.GetValueFromBag(bag, cc);
			}
		}
	}
}