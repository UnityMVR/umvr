using System;
using System.Collections.Generic;
using System.Linq;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;
using pindwin.umvr.Repository;

namespace pindwin.umvr.View.Widgets
{
	public sealed class ModelDropdownLabelsProvider
	{
		private const string NULL = "null";
		
		private readonly Dictionary<Type, List<string>> _labels;
		private readonly Dictionary<Type, IRepository> _repositories;

		public ModelDropdownLabelsProvider(List<IRepository> repositories)
		{
			_repositories = repositories.ToDictionary(k => k.StoredType, v => v);
			_labels = new Dictionary<Type, List<string>>();
		}

		public List<string> GetLabelsForType(Type type)
		{
			return _labels.TryGetValue(type, out List<string> value) ? value : InitializeType(type);
		}

		private IRepository GetRepositoryOfType(Type t)
		{
			return _repositories.TryGetValue(t, out IRepository repository)
				? repository
				: throw new UMVRCantLocateResourceException(t, GetType());
		}

		private List<string> InitializeType(Type type)
		{
			var result = new List<string>();
			_labels[type] = result;
			IRepository repository = GetRepositoryOfType(type);
			repository.CountChanged += () => RefreshType(type);
			RefreshType(type);
			return result;
		}

		private void RefreshType(Type type)
		{
			List<string> list = _labels[type];
			list.Clear();
			list.Add(NULL);
			foreach (IModel model in GetRepositoryOfType(type))
			{
				list.Add(model.ToString());
			}
		}
	}
}