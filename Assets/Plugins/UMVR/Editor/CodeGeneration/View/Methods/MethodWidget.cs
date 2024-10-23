using System.Collections.Generic;
using pindwin.umvr.Editor.CodeGeneration.Designer;
using pindwin.umvr.Editor.CodeGeneration.View.Parameters;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.View.Methods
{
    public class MethodWidget : BindableElement, INotifyValueChanged<DesignerMethod>
    {
        private readonly ParamWidget _signatureWidget;
        private readonly ListView _parametersList;
        
        private List<DesignerParameter> _parameters;

        public MethodWidget()
        {
            if (DesignerUtility.DesignerViewResources.MethodUXML == null)
            {
                return;
            }
            
            DesignerUtility.DesignerViewResources.MethodUXML.CloneTree(this);
            this.style.flexGrow = 1.0f;
            
            _signatureWidget = this.Q<ParamWidget>("method-param-signature");
            _signatureWidget.RegisterValueChangedCallback(evt =>
            {
                value = new DesignerMethod(evt.newValue.Name, evt.newValue.Type, _parameters);
            });
            
            _parametersList = this.Q<ListView>("method-listView-params");
            InitializeListView<ParamWidget>(DesignerUtility.DesignerViewResources.ParameterUXML);
        }
        
        public string MethodName
        {
            get => _signatureWidget?.ParamName;
            set
            {
                if (_signatureWidget != null)
                {
                    _signatureWidget.ParamName = value;
                }
            }
        }
        
        public string MethodReturnType
        {
            get => _signatureWidget?.ParamType;
            set
            {
                if (_signatureWidget != null)
                {
                    _signatureWidget.ParamType = value;
                }
            }
        }
        
        public List<DesignerParameter> Parameters
        {
            get => _parameters;
            set
            {
                _parameters = value;
                _parametersList.itemsSource = value;
            }
        }
        
        private void InitializeListView<TWidgetType>(VisualTreeAsset elementUXML) 
            where TWidgetType : VisualElement, INotifyValueChanged<DesignerParameter>
        {
            _parametersList.showAddRemoveFooter = true;
            _parametersList.makeItem = () =>
            {
                VisualElement element = new VisualElement();
                elementUXML.CloneTree(element);
                element.style.flexGrow = new StyleFloat(1);
                return element;
            };

            _parametersList.bindItem = (e, i) =>
            {
                var w = e.Q<TWidgetType>();
                w.SetValueWithoutNotify(_parameters[i]);
            };
			
            _parametersList.style.flexDirection = FlexDirection.Column;
            _parametersList.style.flexGrow = 1.0f;

            _parametersList.selectionType = SelectionType.Single;
        }

        public DesignerMethod value
        {
            get => new (MethodName, MethodReturnType, _parameters);
            set
            {
                using var pooled = ChangeEvent<DesignerMethod>.GetPooled(
                    new DesignerMethod(MethodName, MethodReturnType, _parameters),
                    value);

                pooled.target = this;
                SetValueWithoutNotify(value);
                SendEvent(pooled);
            }
        }
        
        public void SetValueWithoutNotify(DesignerMethod newValue)
        { 
            MethodName = newValue?.Name ?? string.Empty;
            MethodReturnType = newValue?.Type ?? string.Empty;
            _parameters = newValue?.Parameters ?? new List<DesignerParameter>();
			
            MarkDirtyRepaint();
        }
        
        public new class UxmlFactory : UxmlFactory<MethodWidget, UxmlTraits> { }
        
        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _name = new() { name = "method-name" };
            private readonly UxmlStringAttributeDescription _type = new() { name = "method-return-type" };

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

                if (ve is not MethodWidget param)
                {
                    return;
                }

                param.MethodName = _name.GetValueFromBag(bag, cc);
                param.MethodReturnType = _type.GetValueFromBag(bag, cc);
            }
        }
    }
}
