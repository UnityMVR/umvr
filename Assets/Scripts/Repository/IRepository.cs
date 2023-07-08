using System;
using System.Collections.Generic;
using pindwin.umvr.Model;

namespace pindwin.umvr.Repository
{
	public interface IRepository<TEntity> : IRepository
		where TEntity : IModel
	{
		IReadOnlyList<TEntity> Entities { get; }

		event Action<TEntity> Added;
		event Action<TEntity> Removed;

		void Add(TEntity model);
		void Remove(TEntity model);

		TEntity Get(Id id);
		TEntity GetBy(string indexName, object id);
	}

	public interface IRepository : IDisposable, IEnumerable<IModel>
	{
		bool SuppressNotifications { get; set; }
		void Add(IModel model);
		void Remove(IModel model);
	}
}