using System.Collections.Generic;
using pindwin.umvr.Editor.CodeGeneration.Designer;
using pindwin.umvr.Editor.CodeGeneration.View.Parameters;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.View.Methods
{
    public class MethodWidget : BindableElement, INotifyValueChanged<DesignerMethod>
    {
        private readonly UnconstrainedParamWidget _signatureWidget;
        private readonly ListView _parametersList;
        private readonly Foldout _foldout;
        
        private List<DesignerSignature> _parameters;

        public MethodWidget(string type, string name)
        {
            if (DesignerUtility.DesignerViewResources.MethodUXML == null)
            {
                return;
            }
            
            DesignerUtility.DesignerViewResources.MethodUXML.CloneTree(this);
            style.flexGrow = 1.0f;
            
            _foldout = this.Q<Foldout>("method-blueprint");
            
            _signatureWidget = this.Q<UnconstrainedParamWidget>("method-param-signature");
            _signatureWidget.ParamType = type;
            _signatureWidget.ParamName = _foldout.text = name;
            _signatureWidget.RegisterValueChangedCallback(evt =>
            {
                value = new DesignerMethod(evt.newValue.Name, evt.newValue.Type, _parameters);
                _foldout.text = evt.newValue.Name;
            });
            
            _parametersList = this.Q<ListView>("method-listView-params");
            InitializeListView();
        }
        
        public MethodWidget() : this(string.Empty, string.Empty)
        { }
        
        public string MethodName
        {
            get => _signatureWidget?.ParamName;
            set
            {
                if (_signatureWidget != null)
                {
                    _signatureWidget.ParamName = value;
                }

                if (_foldout != null)
                {
                    _foldout.text = value;
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
        
        public List<DesignerSignature> Parameters
        {
            get => _parameters;
            set
            {
                _parameters = value;
                _parametersList.itemsSource = value;
            }
        }
        
        private void InitializeListView()
        {
            _parametersList.showAddRemoveFooter = true;
            _parametersList.makeItem = () =>
            {
                VisualElement element = new UnconstrainedParamWidget();
                element.style.flexGrow = new StyleFloat(1);
                return element;
            };

            _parametersList.bindItem = (e, i) =>
            {
                var w = e.Q<UnconstrainedParamWidget>() as INotifyValueChanged<DesignerSignature>;
                w.SetValueWithoutNotify(_parameters[i]);
                w.RegisterValueChangedCallback(RefreshParameter);
            };
            
            _parametersList.unbindItem = (e, _) =>
            {
                var w = e.Q<UnconstrainedParamWidget>();
                w.UnregisterValueChangedCallback(RefreshParameter);
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
            _parameters = newValue?.Parameters ?? new List<DesignerSignature>();
			
            MarkDirtyRepaint();
        }

        private void RefreshParameter(ChangeEvent<DesignerSignature> parameter)
        {
            int index = _parameters.FindIndex(p => p == parameter.previousValue);
            _parameters[index] = parameter.newValue;
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
