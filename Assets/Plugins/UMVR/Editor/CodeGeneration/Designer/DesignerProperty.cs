using pindwin.umvr.Attributes;

namespace pindwin.umvr.Editor.CodeGeneration.Designer
{
    public class DesignerProperty
    {
        public DesignerProperty(
            string type, 
            string name, 
            bool isCollection, 
            InitializationLevel initializationLevel,
            bool isReadOnly, 
            bool customImplementation, 
            bool genericProperty)
        {
            Type = type;
            Name = name;
            IsCollection = isCollection;
            InitializationLevel = initializationLevel;
            IsReadOnly = isReadOnly;
            CustomImplementation = customImplementation;
            GenericProperty = genericProperty;
        }

        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsCollection { get; set; }
        public InitializationLevel InitializationLevel { get; set; }
        public bool IsReadOnly { get; set; }
        public bool CustomImplementation { get; set; }
        public bool GenericProperty { get; set; }
    }
}