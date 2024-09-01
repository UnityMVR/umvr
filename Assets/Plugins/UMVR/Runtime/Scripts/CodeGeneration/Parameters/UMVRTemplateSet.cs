using System.Collections.Generic;
using System.IO;
using pindwin.development;
using UnityEditor;
using UnityEngine;
using ILogger = pindwin.development.ILogger;

namespace GenerationParams
{
	[CreateAssetMenu(fileName = nameof(UMVRTemplateSet), menuName = "UMVR/Template Set")]
	public class UMVRTemplateSet : ScriptableObject
	{
		private static readonly string[] _extensions = { ".tt", ".t4", ".ttinclude", ".txt" };
		private const string Assets = "Assets/";
		
		[SerializeField] private TextAsset _modelBaseTemplate;
		[SerializeField] private TextAsset _modelStubTemplate;
		[SerializeField] private TextAsset _installerTemplate;
		[SerializeField] private TextAsset _reactorBaseTemplate;
		[SerializeField] private TextAsset _reactorStubTemplate;
		[SerializeField] private TextAsset _repositoryStubTemplate;
		[SerializeField] private TextAsset _factoryStubTemplate;
		[SerializeField] private List<string> _ensureFilesExistence;

		public TextAsset ModelBaseTemplate => _modelBaseTemplate;
		public TextAsset ModelStubTemplate => _modelStubTemplate;
		public TextAsset InstallerTemplate => _installerTemplate;
		public TextAsset ReactorBaseTemplate => _reactorBaseTemplate;
		public TextAsset ReactorStubTemplate => _reactorStubTemplate;
		public TextAsset RepositoryStubTemplate => _repositoryStubTemplate;
		public TextAsset FactoryStubTemplate => _factoryStubTemplate;

		public string GetPath(TextAsset asset)
		{
			string assetPath = AssetDatabase.GetAssetPath(asset);
			string projectPath = Application.dataPath.Remove(Application.dataPath.Length - Assets.Length);
			foreach (string extension in _extensions)
			{
				string path = Path.Combine(projectPath, Path.GetDirectoryName(assetPath) ?? string.Empty, $"{asset.name}{extension}");
				if (File.Exists(path))
				{
					return path;
				}
			}

			return null;
		}

		private bool EnsureFilesExistence(ILogger logger)
		{
			string assetPath = AssetDatabase.GetAssetPath(this);
			for (int i = 0; i < _ensureFilesExistence.Count; i++)
			{
				string fileName = _ensureFilesExistence[i];
				if (EnsureFileAtPath(assetPath, fileName))
				{
					continue;
				}

				logger.Log($"Missing file in {typeof(UMVRTemplateSet)}: {fileName} does not exist near {assetPath}",
						   LogSeverity.Error);
				return false;
			}

			return true;
		}

		private static bool EnsureFileAtPath(string assetPath, string fileName)
		{
			string path = Path.Combine(Path.GetDirectoryName(assetPath) ?? string.Empty, fileName);
			return File.Exists(path);
		}

		public bool Validate(ILogger logger)
		{
			return
				CheckAssetValidity(ModelBaseTemplate, nameof(ModelBaseTemplate)) &&
				CheckAssetValidity(ModelStubTemplate, nameof(ModelStubTemplate)) &&
				CheckAssetValidity(InstallerTemplate, nameof(InstallerTemplate)) &&
				CheckAssetValidity(ReactorBaseTemplate, nameof(ReactorBaseTemplate)) &&
				CheckAssetValidity(ReactorStubTemplate, nameof(ReactorStubTemplate)) &&
				CheckAssetValidity(RepositoryStubTemplate, nameof(RepositoryStubTemplate)) &&
				CheckAssetValidity(FactoryStubTemplate, nameof(FactoryStubTemplate)) &&
				EnsureFilesExistence(logger);
			

			bool CheckAssetValidity(TextAsset asset, string fieldName)
			{
				if (asset == null)
				{
					logger.Log($"Missing file in {typeof(UMVRTemplateSet)}: {fieldName} was null!", LogSeverity.Error);
					return false;
				}

				string assetPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(asset));
				string localPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

				if (localPath != assetPath)
				{
					logger.Log($"Improper placement of {fieldName} in {typeof(UMVRTemplateSet)}: expected at path {localPath}, was at path {assetPath}", LogSeverity.Error);
					return false;
				}

				return true;
			}
		}
	}
}