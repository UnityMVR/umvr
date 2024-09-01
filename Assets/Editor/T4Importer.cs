using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace pindwin.umvr.Editor
{
	[ScriptedImporter(1, new string[] { "tt", "t4", "ttinclude" })]
	public class T4Importer : ScriptedImporter
	{
		public override void OnImportAsset(AssetImportContext ctx)
		{
			TextAsset subAsset = new TextAsset(File.ReadAllText(ctx.assetPath));
			ctx.AddObjectToAsset("text", subAsset);
			ctx.SetMainObject(subAsset);
		}
	}
}