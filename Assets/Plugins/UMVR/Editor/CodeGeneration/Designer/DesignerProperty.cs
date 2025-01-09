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
        
        public DesignerProperty(DesignerProperty property) : this(property.Type, property.Name, property.IsCollection, property.InitializationLevel, property.IsReadOnly, property.CustomImplementation, property.GenericProperty)
        { }

        public DesignerProperty(): this(string.Empty, string.Empty, false, InitializationLevel.Default, false, false, false)
        { }

        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsCollection { get; set; }
        public InitializationLevel InitializationLevel { get; set; }
        public bool IsReadOnly { get; set; }
        public bool CustomImplementation { get; set; }
        public bool GenericProperty { get; set; }

        public override string ToString()
        {
            return $"{Type} {Name} ({(IsCollection ? "Collection" : "Single")})";
        }
    }
}