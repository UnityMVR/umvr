namespace pindwin.umvr.Plugins.UMVR.Editor.CodeGeneration.Designer
{
	public class DesignerParameter
	{
		public DesignerParameter(string name, string type)
		{
			Type = type;
			Name = name;
		}
		
		public string Type { get; set; }
		public string Name { get; set; }
	}
}