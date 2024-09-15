using System;
using System.Collections.Generic;
using System.Linq;
using pindwin.umvr.Editor.Extensions;
using pindwin.umvr.Model;

namespace pindwin.umvr.Plugins.UMVR.Editor.CodeGeneration.Designer
{
	public static class DesignerUtility
	{
		public static IReadOnlyList<Type> ModelTypes { get; }
		
		public static IReadOnlyList<Type> SimpleTypes { get; }
		
		public static IReadOnlyList<string> LegalTypeNames { get; }
		
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
		}
	}
}