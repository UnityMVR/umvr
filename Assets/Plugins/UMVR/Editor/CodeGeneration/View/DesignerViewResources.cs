using UnityEngine;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.Window
{
    [CreateAssetMenu(menuName = "UMVR/" + nameof(DesignerViewResources), fileName = nameof(DesignerViewResources))]
    public class DesignerViewResources : ScriptableObject
    {
        [SerializeField] private VisualTreeAsset _parameterUXML;
        [SerializeField] private VisualTreeAsset _propertyUXML;
        [SerializeField] private VisualTreeAsset _methodUXML;
        [SerializeField] private VisualTreeAsset _assistedTextFieldUXML;

        public VisualTreeAsset ParameterUXML => _parameterUXML;
        public VisualTreeAsset PropertyUXML => _propertyUXML;
        public VisualTreeAsset MethodUXML => _methodUXML;
        public VisualTreeAsset AssistedTextField => _assistedTextFieldUXML;
    }
}