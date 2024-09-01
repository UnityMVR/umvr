using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Threading.Tasks;
using pindwin.umvr.Editor.Generation;

namespace pindwin.umvr.Editor.CodeGeneration
{
	public class CodeGenerator : ICodeGenerator
	{
		public IGenerationProcessedResult Run(ICodeGeneratorSession session)
		{
			var genProcessedResult = new GenerationProcessedResult();
			var templateGenerator = new UMVRTemplateGenerator(session);
			
			foreach (FileGenerationRequest request in session.Requests.Values)
			{
				if (request.IsRequested)
				{
					genProcessedResult.Add(ProcessFile(request, templateGenerator));
				}
			}

			return genProcessedResult;
		}

		private static void EnsureDirectoryExists(string path)
		{
			if (Directory.Exists(path) == false)
			{
				Directory.CreateDirectory(path);
			}
		}

		private static IFileGenerationResult ProcessFile(FileGenerationRequest request, UMVRTemplateGenerator tg)
		{
			var result = new FileGenerationResult(request);
			string path = default;
			try
			{
				EnsureDirectoryExists(request.EnsurePath);
				result.WasSuccess =
					tg.ProcessTemplate(request.InputPath, File.ReadAllText(request.InputPath), ref path, out string output);
				result.Text = output;
			}
			catch (Exception e)
			{
				result.AddError($"Failed to generate {request.OutputPath}: {e}");
				result.WasSuccess = false;
			}
			finally
			{
				foreach (CompilerError error in tg.Errors)
				{
					if (error.IsWarning)
					{
						continue;
					}

					result.AddError($"ln.{error.Line}: {error.ErrorText} ({error.ErrorNumber})");
				}
			}

			return result;
		}
	}
}