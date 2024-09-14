using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[assembly: UxmlNamespacePrefix("pindwin.umvr.Editor.CodeGeneration.View", "view")]

namespace pindwin.umvr.Editor.CodeGeneration.View.Parameters
{
	public class Param : BindableElement, INotifyValueChanged<string>
	{
		public Param()
		{
			ParamType = nameof(Int32);
			ParamName = string.Empty;
			
			_typeField = new DropdownField(string.Empty)
			{
				name = "param-type-dropdown"
			};
			_typeField.AddToClassList(DropdownField.ussClassName);
			Add(_typeField);

			_nameField = new TextField(string.Empty)
			{
				name = "param-name-textfield"
			};
			_nameField.AddToClassList(TextField.ussClassName);
			Add(_nameField);
			
			style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
			style.flexGrow = new StyleFloat(1.0f);
			
			style.height = new StyleLength(20.0f);
			style.maxHeight = new StyleLength(20.0f);
		}

		public string ParamType { get; set; }
		public string ParamName { get; set; }

		private DropdownField _typeField;
		private TextField _nameField;

		public void SetValueWithoutNotify(string newValue)
		{
			ParamName = newValue;
			MarkDirtyRepaint();
		}

		string INotifyValueChanged<string>.value
		{
			get => ParamName;
			set
			{
				using ChangeEvent<string> pooled = ChangeEvent<string>.GetPooled(ParamName, value);
				
				pooled.target = this;
				SetValueWithoutNotify(value);
				SendEvent(pooled);
			}
		}
		
		public new class UxmlFactory : UxmlFactory<Param, UxmlTraits> 
		{ }
		
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			private readonly UxmlStringAttributeDescription ParamType = new() { name = "param-type" };
			private readonly UxmlStringAttributeDescription ParamName = new() { name = "param-name" };
			
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get { yield break; }
			}

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				
				if (ve is not Param param)
				{
					return;
				}
				
				param.ParamType = ParamType.GetValueFromBag(bag, cc);
				param.ParamName = ParamName.GetValueFromBag(bag, cc);
			}
		}
	}
}