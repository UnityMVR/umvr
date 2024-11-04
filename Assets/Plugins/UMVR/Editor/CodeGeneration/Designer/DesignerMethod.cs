using System.Collections.Generic;

namespace pindwin.umvr.Editor.CodeGeneration.Designer
{
    public class DesignerMethod
    {
        public DesignerMethod(string name, string type) : this(name, type, new List<DesignerSignature>())
        { }
        
        public DesignerMethod(string name, string type, List<DesignerSignature> parameters)
        {
            Type = type;
            Name = name;
            
            Parameters = parameters;
        }
        
        public string Type { get; set; }
        public string Name { get; set; }
        public List<DesignerSignature> Parameters { get; set; }
    }
}