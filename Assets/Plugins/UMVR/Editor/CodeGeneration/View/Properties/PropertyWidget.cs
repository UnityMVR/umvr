using System;
using System.Collections.Generic;
using pindwin.umvr.Attributes;
using pindwin.umvr.Editor.CodeGeneration.Designer;
using pindwin.umvr.Editor.CodeGeneration.View.Parameters;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.View.Properties
{
    public class PropertyWidget : BindableElement, INotifyValueChanged<DesignerProperty>
    {
        private readonly ParamWidget _paramWidget;
        private readonly Toggle _isCollectionToggle;
        private readonly DropdownField _initializationLevelField;
        private readonly Toggle _isReadOnlyToggle;
        private readonly Toggle _customImplementationToggle;
        private readonly Toggle _genericPropertyToggle;
        
        private DesignerProperty _value;
        
        public PropertyWidget()
        {
            DesignerUtility.DesignerViewResources.PropertyUXML.CloneTree(this);
            
            _paramWidget = this.Q<ParamWidget>("PropertyBlueprint-metadata-param");
            _paramWidget.RegisterValueChangedCallback(evt =>
            {
                PropertyName = evt.newValue.Name;
                PropertyType = evt.newValue.Type;
            });
            
            _isCollectionToggle = this.Q<Toggle>("PropertyBlueprint-is-collection-toggle");
            _initializationLevelField = this.Q<DropdownField>("PropertyBlueprint-initialization-level-dropdown");
            _isReadOnlyToggle = this.Q<Toggle>("PropertyBlueprint-is-read-only-toggle");
            _customImplementationToggle = this.Q<Toggle>("PropertyBlueprint-custom-implementation-toggle");
            _genericPropertyToggle = this.Q<Toggle>("PropertyBlueprint-generic-property-toggle");
            
            _value = new DesignerProperty();
        }

        private void RefreshValue()
        {
            (this as INotifyValueChanged<DesignerProperty>).value = new DesignerProperty(
                PropertyName, PropertyType, IsCollection, InitializationLevel, IsReadOnly, CustomImplementation,
                GenericProperty);
        }

        public string PropertyName
        {
            get => _paramWidget?.ParamName;
            set
            {
                _value ??= new DesignerProperty();
                
                if (_paramWidget != null)
                {
                    _paramWidget.ParamName = value;
                }
                
                _value.Name = value;
                RefreshValue();
            }
        }

        public string PropertyType
        {
            get => _paramWidget?.ParamType;
            set
            {
                _value ??= new DesignerProperty();
                
                if (_paramWidget != null)
                {
                    _paramWidget.ParamType = value;
                }
                
                _value.Type = value;
                RefreshValue();
            }
        }

        public bool IsCollection
        {
            get => _isCollectionToggle?.value ?? false;
            set
            {
                if (_isCollectionToggle != null)
                {
                    _isCollectionToggle.value = value;
                }
            } 
        }

        public InitializationLevel InitializationLevel
        {
            get => Enum.TryParse<InitializationLevel>(_initializationLevelField?.value, out var level) ? level : InitializationLevel.Default;
            set 
            {
                if (_initializationLevelField != null)
                {
                    _initializationLevelField.value = value.ToString();
                } 
            }
        }

        public bool IsReadOnly
        {
            get => _isReadOnlyToggle?.value ?? false;
            set
            {
                if (_isReadOnlyToggle != null)
                {
                    _isReadOnlyToggle.value = value;
                }
            }
        }

        public bool CustomImplementation
        {
            get => _customImplementationToggle?.value ?? false;
            set
            {
                if (_customImplementationToggle != null)
                {
                    _customImplementationToggle.value = value;
                }
            }
        }

        public bool GenericProperty
        {
            get => _genericPropertyToggle?.value ?? false;
            set
            {
                if (_genericPropertyToggle != null)
                {
                    _genericPropertyToggle.value = value;
                }
            }
        }

        DesignerProperty INotifyValueChanged<DesignerProperty>.value
        {
            get => _value;
            set
            {
                using ChangeEvent<DesignerProperty> pooled = ChangeEvent<DesignerProperty>.GetPooled(
                    _value, 
                    value);

                pooled.target = this;
                SetValueWithoutNotify(value);
                SendEvent(pooled);
            }
        }

        public void SetValueWithoutNotify(DesignerProperty newValue)
        {
            _value = newValue;
            
            PropertyName = newValue?.Name ?? string.Empty;
            PropertyType = newValue?.Type ?? string.Empty;
            IsCollection = newValue?.IsCollection ?? false;
            InitializationLevel = newValue?.InitializationLevel ?? InitializationLevel.Default;
            IsReadOnly = newValue?.IsReadOnly ?? false;
            CustomImplementation = newValue?.CustomImplementation ?? false;
            GenericProperty = newValue?.GenericProperty ?? false;
			
            MarkDirtyRepaint();
        }
        
        public new class UxmlFactory : UxmlFactory<PropertyWidget, UxmlTraits> { }

        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _name = new() { name = "property-name" };
            private readonly UxmlStringAttributeDescription _type = new() { name = "property-type" };
            private readonly UxmlBoolAttributeDescription _isCollection = new() { name = "is-collection" };
            private readonly UxmlStringAttributeDescription _initializationLevel = new() { name = "initialization-level" };
            private readonly UxmlBoolAttributeDescription _isReadOnly = new() { name = "is-read-only" };
            private readonly UxmlBoolAttributeDescription _customImplementation = new() { name = "custom-implementation" };
            private readonly UxmlBoolAttributeDescription _genericProperty = new() { name = "generic-property" };

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

                if (ve is not PropertyWidget param)
                {
                    return;
                }

                param.PropertyName = _name.GetValueFromBag(bag, cc);
                param.PropertyType = _type.GetValueFromBag(bag, cc);
                param.IsCollection = _isCollection.GetValueFromBag(bag, cc);
                param.InitializationLevel = Enum.TryParse<InitializationLevel>(_initializationLevel.GetValueFromBag(bag, cc), out var level) ? level : InitializationLevel.Default;
                param.IsReadOnly = _isReadOnly.GetValueFromBag(bag, cc);
                param.CustomImplementation = _customImplementation.GetValueFromBag(bag, cc);
                param.GenericProperty = _genericProperty.GetValueFromBag(bag, cc);
            }
        }
    }
}