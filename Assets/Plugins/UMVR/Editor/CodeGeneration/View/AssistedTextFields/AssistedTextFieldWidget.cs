using System.Collections.Generic;
using System.Linq;
using pindwin.umvr.Editor.CodeGeneration.Designer;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.View.AssistedTextFields
{
	public class AssistedTextFieldWidget : BindableElement, INotifyValueChanged<string>
	{
		private readonly TextField _input;
		private readonly VisualElement _assistanceGroup;
		private readonly ListView _proposedValues;

		private List<string> _options;

		public AssistedTextFieldWidget()
		{
			if (DesignerUtility.DesignerViewResources.AssistedTextField == null)
			{
				return;
			}
			
			DesignerUtility.DesignerViewResources.AssistedTextField.CloneTree(this);

			_input = this.Q<TextField>("atf-text-field");
			_assistanceGroup = this.Q<VisualElement>("atf-selection-root");
			_proposedValues = this.Q<ListView>("atf-proposed-items");
			
			InitializeListView();
		}

		public void RefreshListDisplay()
		{
			_assistanceGroup.style.display =
				_input.value.Length >= GracePeriodLength && _options.Count > 0
					? DisplayStyle.Flex
					: DisplayStyle.None;
			
			_proposedValues.RefreshItems();
		}
		
		public string TextValue
		{
			get => _input.value;
			set => _input.value = value;
		}

		public void SetValueWithoutNotify(string newValue)
		{
			_input.SetValueWithoutNotify(newValue ?? string.Empty);
		}

		string INotifyValueChanged<string>.value
		{
			get => _input.value;
			set
			{
				using var pooled = ChangeEvent<string>.GetPooled(_input.value, value);

				pooled.target = this;
				SetValueWithoutNotify(value);
				SendEvent(pooled);
			}
		}

		public int GracePeriodLength { get; set; }
        
		public List<string> Options
		{
			get => _options;
			set
			{
				_options = value;
				_proposedValues.itemsSource = value;
			}
		}
		
		private void InitializeListView()
		{
			_proposedValues.itemsSource = new List<string>();
			
			_proposedValues.makeItem = () =>
			{
				VisualElement element = new Label();
				element.style.flexGrow = new StyleFloat(0.0f);
				return element;
			};

			_proposedValues.bindItem = (e, i) =>
			{
				var w = e.Q<Label>() as INotifyValueChanged<string>;
				w.SetValueWithoutNotify(_proposedValues.itemsSource[i].ToString());
				
				_proposedValues.MarkDirtyRepaint();
			};
			
			_proposedValues.unbindItem = (e, i) =>
			{
				_proposedValues.MarkDirtyRepaint();
			};

			_proposedValues.onSelectionChange += o =>
			{
				SetValueWithoutNotify(o.Single().ToString());
				_assistanceGroup.style.display = DisplayStyle.None;
			};
			
			_proposedValues.style.flexDirection = FlexDirection.Column;
			_proposedValues.style.flexGrow = 1.0f;

			_proposedValues.selectionType = SelectionType.Single;
		}
		
		public new class UxmlFactory : UxmlFactory<AssistedTextFieldWidget, UxmlTraits> { }
		
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			private readonly UxmlIntAttributeDescription _gracePeriod = new() { name = "grace-period-length" };

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

				if (ve is not AssistedTextFieldWidget param)
				{
					return;
				}

				param.GracePeriodLength = _gracePeriod.GetValueFromBag(bag, cc);
			}
		}
	}
}