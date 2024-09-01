using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GenerationParams;
using pindwin.development;
using pindwin.umvr.Editor.Extensions;
using pindwin.umvr.Editor.Generation;
using pindwin.umvr.Model;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ILogger = pindwin.development.ILogger;

namespace pindwin.umvr.Editor.CodeGeneration.Window
{
	public class CodeGenWindowSession : ScriptableObject, ICodeGeneratorSession
	{
		public string LastOutput;
		public string SelectedTypeString;
		
		[SerializeField] private UMVRTemplateSet _usedTemplate;
		[SerializeField] private List<string> _availableModelNames;

		private List<Type> _availableModels;
		private Type _selectedType;
		private CodeGeneratorSession _session;
		
		public List<string> TypeChoices => _availableModelNames;
		public List<string> GenerateMembersChoices => Enum.GetNames(typeof(GenerateMembersMode)).ToList();
		public GenerateMembersMode GenerateMembersMode { get; private set; }
		public GenSettings GenSettings => _session.GenSettings;
		public IReadOnlyDictionary<string, FileGenerationRequest> Requests => _session?.Requests;
		public bool IsValid => _selectedType != null && _usedTemplate != null;

		public void Process()
		{
			CodeGenerator gen = new CodeGenerator();
			IGenerationProcessedResult processedResult = gen.Run(_session);

			bool isDirty = false;
			foreach (IFileGenerationResult result in processedResult.ProcessedFiles)
			{
				if (result.ResultInfo == GenerationResultInfo.Success)
				{
					File.WriteAllText(result.Path, result.Text);
					isDirty = true;
				}
			}
			
			RefreshOutput(processedResult);

			if (isDirty)
			{
				AssetDatabase.Refresh();
			}
		}

		public void Refresh()
		{
			_availableModels = typeof(IModel).GetMatchingInterfaces();
			_availableModels.Insert(0, null);
			_availableModelNames = _availableModels.Select(t => t?.Name ?? "None").ToList();

			int index = _availableModelNames.IndexOf(SelectedTypeString);
			if (_availableModels.IsValidIndex(index))
			{
				_selectedType = _availableModels[index];
				if (IsValid)
				{
					CreateSessionForType();
				}
			}
		}

		public void OnModelChanged(ChangeEvent<string> e)
		{
			SelectedTypeString = e.newValue;
			int index = _availableModelNames.IndexOf(SelectedTypeString);
			_selectedType = _availableModels.IsValidIndex(index) ? _availableModels[index] : null;
			if (IsValid)
			{
				CreateSessionForType();
			}
		}
		
		public void OnTemplateSetChanged(SerializedPropertyChangeEvent e)
		{
			_usedTemplate = (UMVRTemplateSet)e.changedProperty.objectReferenceValue;
			
			if (IsValid)
			{
				CreateSessionForType();
			}
		}

		public void OnGenerateMembersModeChanged(ChangeEvent<int> e)
		{
			GenerateMembersMode = (GenerateMembersMode)e.newValue;
			
			if (IsValid == false)
			{
				return;
			}
			
			switch (GenerateMembersMode)
			{
				case GenerateMembersMode.Essentials:
					Requests.Values.ToList().ForEach(r => r.IsRequested = r.IsEssential);
					break;
				case GenerateMembersMode.Everything:
					Requests.Values.ToList().ForEach(r => r.IsRequested = true);
					break;
				case GenerateMembersMode.Custom:
					Requests.Values.ToList().ForEach(r => r.IsRequested = false);
					break;
			}
		}

		private void CreateSessionForType()
		{
			string rootPath = PathUtilities.GetRootPath(SelectedTypeString);
			string generatedPath = $"{rootPath}/Generated";

			string className = _selectedType.MakeClassName();
			var requests = new Dictionary<string, FileGenerationRequest>
			{
				{
					"ReactorBase", ConstructRequest("ReactorBase",generatedPath,
													_usedTemplate.GetPath(_usedTemplate.ReactorBaseTemplate),
													$"{generatedPath}/{className}Reactor.cs", true)
				},
				{
					"ReactorStub", ConstructRequest("ReactorStub", rootPath,
													_usedTemplate.GetPath(_usedTemplate.ReactorStubTemplate),
													$"{rootPath}/{className}Reactor.cs")
				},
				{
					"Model", ConstructRequest("Model", generatedPath,
											  _usedTemplate.GetPath(_usedTemplate.ModelBaseTemplate),
											  $"{generatedPath}/{className}Model.cs", true)
				},
				{
					"ModelStub", ConstructRequest("ModelStub", rootPath,
												  _usedTemplate.GetPath(_usedTemplate.ModelStubTemplate),
												  $"{rootPath}/{className}Model.cs")
				},
				{
					"Installer", ConstructRequest("Installer", generatedPath,
												  _usedTemplate.GetPath(_usedTemplate.InstallerTemplate),
												  $"{generatedPath}/{className}Installer.cs", true)
				},
				{
					"FactoryStub", ConstructRequest("FactoryStub", rootPath,
													_usedTemplate.GetPath(_usedTemplate.FactoryStubTemplate),
													$"{rootPath}/{className}Factory.cs")
				},
				{
					"RepositoryStub", ConstructRequest("RepositoryStub", rootPath,
													   _usedTemplate.GetPath(_usedTemplate.RepositoryStubTemplate),
													   $"{rootPath}/{className}Repository.cs")
				}
			};

			var settings = new GenSettings(
				_selectedType.MakeClassName(), 
				_selectedType.Namespace, 
				_selectedType, 
				requests.ToDictionary(k => k.Key, v => (ILogger)v.Value));

			_session = new CodeGeneratorSession(rootPath, requests, settings);
		}

		private void RefreshOutput(IGenerationProcessedResult result)
		{
			StringBuilder sb = new StringBuilder();
			foreach (IFileGenerationResult generationResult in result.ProcessedFiles)
			{
				if (generationResult.HasErrors)
				{
					foreach (string resultError in generationResult.Errors)
					{
						sb.AppendLine($"<color=orange>{resultError}</color>");
					}
					continue;
				}

				sb.AppendLine($"{generationResult.Request.Name} generated successfully!");
			}

			LastOutput = sb.ToString();
		}

		private static FileGenerationRequest ConstructRequest(
			string name,
			string ensurePath,
			string inputPath,
			string outputPath,
			bool isEssential = false)
		{
			var request = CreateInstance<FileGenerationRequest>();
			request.Name = name;
			request.IsRequested = false;
			request.IsEssential = isEssential;
			request.EnsurePath = ensurePath;
			request.InputPath = inputPath;
			request.OutputPath = outputPath;
			return request;
		}
	}
}