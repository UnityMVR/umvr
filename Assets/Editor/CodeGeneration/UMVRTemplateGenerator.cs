using GenerationParams;
using Mono.TextTemplating;
using pindwin.umvr.Attributes;
using pindwin.umvr.Editor.CodeGeneration;

namespace pindwin.umvr.Editor.Generation
{
	public class UMVRTemplateGenerator : TemplateGenerator
	{
		public GenSettings Settings { get; set; }
		
		public UMVRTemplateGenerator(ICodeGeneratorSession session)
		{
			Refs.Add(typeof(GenSettings).Assembly.Location);
			Refs.Add(typeof(UMVRTemplateGenerator).Assembly.Location);
			Refs.Add(typeof(InitializationAttribute).Assembly.Location);
			Imports.Add("pindwin.umvr.Editor.Generation");
			Imports.Add("pindwin.umvr.Attributes");
			Settings = session.GenSettings;
		}
	}
}