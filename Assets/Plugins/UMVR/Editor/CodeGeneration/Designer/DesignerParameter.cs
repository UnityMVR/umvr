namespace pindwin.umvr.Editor.CodeGeneration.Designer
{
	public class DesignerParameter
	{
		public DesignerParameter(string name, string type)
		{
			Type = type;
			Name = name;
		}
		
		public DesignerParameter() : this(string.Empty, string.Empty)
		{ }
		
		public string Type { get; set; }
		public string Name { get; set; }
	}
}