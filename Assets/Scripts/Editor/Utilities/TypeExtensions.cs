using System;
using System.Collections.Generic;
using System.Reflection;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;

namespace pindwin.umvr.Editor.Extensions
{
	public static class TypeExtensions
	{
		public static string MakeClassName<TModel>()
			where TModel : IModel
		{
			return typeof(TModel).MakeClassName();
		}

		public static string MakeClassName(this Type interfaceType)
		{
			string interfaceName = interfaceType.Name;
			char secondLetter = interfaceName.Length > 1 ? interfaceName[1] : '_';
			if (interfaceName.StartsWith("I") && (secondLetter != '_') && (char.ToLower(secondLetter) != secondLetter))
			{
				return interfaceName.Substring(1);
			}

			throw new UMVRException(
				"Code gen templates expect interface name in convention I{Name}, e.x. IExample. "
				+ $"Provided name: {interfaceName}"
			);
		}

		public static List<Type> GetMatchingInterfaces(this Type type)
		{
			List<Type> matchingInterfaces = new List<Type>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				GetMatchingInterfaces(type, assembly, matchingInterfaces);
			}

			return matchingInterfaces;
		}
		
		public static void GetMatchingInterfaces(this Type type, Assembly assembly, List<Type> results)
		{
			if (assembly == type.Assembly)
			{
				return;
			}

			foreach (Type candidate in assembly.GetTypes())
			{
				if (type.IsAssignableFrom(candidate) && candidate.IsInterface)
				{
					results.Add(candidate);
				}
			}
		}
	}
}