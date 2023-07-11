using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace pindwin.umvr.View.Widgets
{
	public class DropdownWidget : DebuggerElement, ISinglePropertyWidget
	{
		public DropdownWidget(List<string> choices)
		{
			style.flexDirection = FlexDirection.Row;
			style.alignItems = Align.FlexStart;
			AddToClassList(PROPERTY_CLASS);

			DropdownField = MakeDropdown(string.Empty, choices, this);

			DropdownField.RegisterValueChangedCallback(OnValueChanged);
		}

		public DropdownWidget(List<string> choices, string label)
			: this(choices)
		{
			Label = label;
		}

		public DropdownWidget(List<string> choices, string label, string value)
			: this(choices, label)
		{
			Value = value;
		}

		public event Action<string> ValueChanged;

		public bool IsReadonly
		{
			get => DropdownField.enabledSelf;
			set
			{
				DropdownField.SetEnabled(value);
				MarkDirtyRepaint();
			}
		}

		public string Label
		{
			get => DropdownField.label;

			set
			{
				DropdownField.label = value;
				MarkDirtyRepaint();
			}
		}

		public string Value
		{
			get => DropdownField.value;
			set => DropdownField.value = value;
		}

		protected DropdownField DropdownField { get; }

		protected virtual string GetChangedValueImpl()
		{
			return DropdownField.value;
		}

		private void OnValueChanged(ChangeEvent<string> e)
		{
			ValueChanged?.Invoke(GetChangedValueImpl());
		}
	}
}