using System;
using UnityEngine.UIElements;

namespace pindwin.umvr.View.Widgets
{
	public class TextFieldWidget : DebuggerElement, ISinglePropertyWidget, IValidatedProperty
	{
		public TextField FieldValue { get; }
		
		private readonly Button _commitButton;
		private readonly Button _revertButton;
		
		private string _value;

		public TextFieldWidget()
		{
			style.flexDirection = FlexDirection.Row;
			style.alignItems = Align.FlexStart;
			AddToClassList(PROPERTY_CLASS);

			FieldValue = MakeTextField();

			Add(FieldValue);

			_commitButton = MakeButton(CommitChangedValue, COMMIT_BUTTON, this);
			_revertButton = MakeButton(RevertChangedValue, REVERT_BUTTON, this);

			FieldValue.RegisterValueChangedCallback(OnValueChanged);
		}

		public TextFieldWidget(string label)
			: this()
		{
			Label = label;
		}

		public TextFieldWidget(string label, string value)
			: this(label)
		{
			Value = value;
		}

		public event Action<string> ValueChanged;
		public event Action<string> ValueProposed; 

		public bool IsReadonly
		{
			get => FieldValue.enabledSelf;
			set
			{
				FieldValue.SetEnabled(value);
				MarkDirtyRepaint();
			}
		}

		public string Label
		{
			get => FieldValue.label;

			set
			{
				FieldValue.label = value;
				MarkDirtyRepaint();
			}
		}

		public bool CanCommit
		{
			get => _commitButton.enabledSelf;
			set => _commitButton.SetEnabled(value);
		}

		protected string TempValue
		{
			get => FieldValue.value;

			private set
			{
				FieldValue.value = value;
				_commitButton.style.display = TempValue != Value ? DisplayStyle.Flex : DisplayStyle.None;
				_revertButton.style.display = _commitButton.style.display;
				MarkDirtyRepaint();
			}
		}

		public string Value
		{
			get => _value;
			set
			{
				_value = value;
				TempValue = value;
			}
		}

		private void CommitChangedValue()
		{
			ValueChanged?.Invoke(GetNotificationValueImpl());
		}

		protected virtual string GetNotificationValueImpl()
		{
			return TempValue;
		}

		private void OnValueChanged(ChangeEvent<string> e)
		{
			TempValue = e.newValue;
			ValueProposed?.Invoke(GetNotificationValueImpl());
		}

		private void RevertChangedValue()
		{
			TempValue = Value;
		}
	}
}