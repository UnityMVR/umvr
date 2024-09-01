using System;
using System.IO;
using UnityEditor;

namespace pindwin.umvr.Editor.Utilities
{
	public class DeletionDetector : AssetModificationProcessor
	{
		public static event Action Deleted; 
		
		static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt)
		{
			File.Delete(path);
			if (File.Exists($"{path}.meta"))
			{
				File.Delete($"{path}.meta");
			}
			Deleted?.Invoke();
			return AssetDeleteResult.DidDelete;
		}
	}
}