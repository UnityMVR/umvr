using System;

namespace pindwin.umvr.Model
{
	internal class ModelCleanupStep : IComparable<ModelCleanupStep>
	{
		public Action<IModel> Action { get; }
		private int Priority { get; }
			
		public ModelCleanupStep(Action<IModel> action, int priority)
		{
			Action = action;
			Priority = priority;
		}

		public int CompareTo(ModelCleanupStep other)
		{
			if (ReferenceEquals(this, other))
			{
				return 0;
			}

			if (ReferenceEquals(null, other))
			{
				return 1;
			}

			return Priority.CompareTo(other.Priority);
		}
	}
}