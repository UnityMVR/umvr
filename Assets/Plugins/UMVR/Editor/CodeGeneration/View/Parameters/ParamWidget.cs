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

		private DesignerParameter _value;
		
		public ParamWidget()
		{
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
			
			_typeField.RegisterValueChangedCallback(evt =>
			{
				ParamType = evt.newValue;
			});
			
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
			
			_nameField.RegisterValueChangedCallback(evt =>
			{
				ParamName = evt.newValue;
			});
			
			Add(_nameField);
		}

		public string ParamName
		{
			get => _value?.Name ?? string.Empty;
			set
			{
				if (_nameField != null)
				{
					_nameField.SetValueWithoutNotify(value);
				}
				
				(this as INotifyValueChanged<DesignerParameter>).value = new DesignerParameter(value, ParamType);
			}
		}

		public string ParamType
		{
			get => _value?.Type ?? string.Empty;
			set
			{
				if (_typeField != null)
				{
					_typeField.SetValueWithoutNotify(value);
				}
				
				(this as INotifyValueChanged<DesignerParameter>).value = new DesignerParameter(ParamName, value);
			}
		}

		DesignerParameter INotifyValueChanged<DesignerParameter>.value
		{
			get => _value;
			set
			{
				using ChangeEvent<DesignerParameter> pooled = ChangeEvent<DesignerParameter>.GetPooled(_value, value);

				pooled.target = this;
				SetValueWithoutNotify(value);
				SendEvent(pooled);
			}
		}

		public void SetValueWithoutNotify(DesignerParameter newValue)
		{
			_value = newValue;
			
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