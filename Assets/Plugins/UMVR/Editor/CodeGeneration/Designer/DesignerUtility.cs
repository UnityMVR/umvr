using System;
using System.Collections.Generic;
using System.Linq;
using pindwin.umvr.Editor.CodeGeneration.Window;
using pindwin.umvr.Editor.Extensions;
using pindwin.umvr.Model;
using UnityEditor;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace pindwin.umvr.Editor.CodeGeneration.Designer
{
	public static class DesignerUtility
	{
		public static IReadOnlyList<Type> ModelTypes { get; }
		public static IReadOnlyList<Type> SimpleTypes { get; }
		public static IReadOnlyList<string> LegalTypeNames { get; }
		internal static DesignerViewResources DesignerViewResources { get; }
		
		static DesignerUtility()
		{
			ModelTypes = typeof(IModel).GetMatchingInterfaces();
			SimpleTypes = new List<Type>
			{
				typeof(Int32),
				typeof(String),
				typeof(Single),
				typeof(Boolean)
			};

			var list = new List<string>();
			list.AddRange(SimpleTypes.Select(t => t.Name));
			list.AddRange(ModelTypes.Select(t => t.Name));
			LegalTypeNames = list;
			
			DesignerViewResources = LoadResource<DesignerViewResources>();
		}

		private static TResource LoadResource<TResource>() where TResource : Object
		{
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(TResource).Name}");
			Assert.AreEqual(1, guids.Length, $"Expected exactly one {typeof(TResource).Name} asset, found {guids.Length}");
			
			return AssetDatabase.LoadAssetAtPath<TResource>(AssetDatabase.GUIDToAssetPath(guids[0]));
		}
	}
}