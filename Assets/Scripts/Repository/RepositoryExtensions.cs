using System;
using System.Collections.Generic;
using pindwin.umvr.Model;

namespace pindwin.umvr.Repository
{
	public static class RepositoryExtensions
	{
		public static TModel FirstOrDefault<TModel>(this IRepository<TModel> Repository, Func<TModel, bool> query)
			where TModel : IModel
		{
			for (int i = 0, length = Repository.Entities.Count; i < length; i++)
			{
				TModel entity = Repository.Entities[i];
				if (query(entity))
				{
					return entity;
				}
			}

			return default;
		}

		public static void WhereNonAlloc<TModel>(
			this IRepository<TModel> repository,
			Func<TModel, bool> query,
			List<TModel> results)
			where TModel : IModel
		{
			results.Clear();
			for (int i = 0, length = repository.Entities.Count; i < length; i++)
			{
				TModel entity = repository.Entities[i];
				if (query(entity))
				{
					results.Add(entity);
				}
			}
		}
	}
}