namespace pindwin.umvr.Editor.CodeGeneration.Designer
{
	public class DesignerSignature
	{
		public DesignerSignature(string name, string type)
		{
			Type = type;
			Name = name;
		}
		
		public string Type { get; set; }
		public string Name { get; set; }

		public override string ToString()
		{
			return $"{Type} {Name}";
		}
	}
}