using UnityEngine;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.Window
{
    [CreateAssetMenu(menuName = "UMVR/" + nameof(DesignerViewResources), fileName = nameof(DesignerViewResources))]
    public class DesignerViewResources : ScriptableObject
    {
        [SerializeField] private VisualTreeAsset _parameterUXML;
        [SerializeField] private VisualTreeAsset _propertyUXML;

        public VisualTreeAsset ParameterUXML => _parameterUXML;
        public VisualTreeAsset PropertyUXML => _propertyUXML;
    }
}