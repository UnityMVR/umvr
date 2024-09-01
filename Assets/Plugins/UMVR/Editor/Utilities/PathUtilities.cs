using UnityEditor;

namespace pindwin.umvr.Editor.Extensions
{
	public static class PathUtilities
	{
		public static string PullOutInterfaceName(string qualifiedName)
		{
			string[] parts = qualifiedName.Split('.');
			return parts[parts.Length - 1];
		}

		public static string GetRootPath(string interfaceName)
		{
			const string extension = "/.cs";
			string[] assets = AssetDatabase.FindAssets(interfaceName);
			for (var i = 0; i < assets.Length; i++)
			{
				string path = AssetDatabase.GUIDToAssetPath(assets[i]);
				string[] parts = path.Split('/');
				if (parts[parts.Length - 1] != $"{interfaceName}.cs")
				{
					continue;
				}

				return path.Remove(path.Length - interfaceName.Length - extension.Length);
			}

			return null;
		}
	}
}