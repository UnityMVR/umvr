using System;

namespace GenerationParams.Utilities
{
	public static class TypeNameExtensions
	{
		public static string GetParamName(this string typeName)
		{
			return $"{char.ToLower(typeName[0])}{typeName.Substring(1)}";
		}

		public static string GetParamName(this Type type)
		{
			if (!type.IsGenericType)
			{
				return GetParamName(type.Name);
			}
			
			string[] parts = type.Name.Split('`');
			return parts[0].GetParamName();
		}
	}
}