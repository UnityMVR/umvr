using System.Collections.Generic;

namespace pindwin.umvr.Editor.CodeGeneration.Designer
{
    public class DesignerMethod
    {
        public DesignerMethod(string name, string type) : this(name, type, new List<DesignerParameter>())
        { }
        
        public DesignerMethod(string name, string type, List<DesignerParameter> parameters)
        {
            Type = type;
            Name = name;
            
            Parameters = parameters;
        }
        
        public string Type { get; set; }
        public string Name { get; set; }
        public List<DesignerParameter> Parameters { get; set; }
    }
}