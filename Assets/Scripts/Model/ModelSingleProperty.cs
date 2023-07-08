namespace pindwin.umvr.Model
{
	public class ModelSingleProperty<TModel> : SingleProperty<TModel>
		where TModel : class, IModel
	{
		public ModelSingleProperty()
		{ }

		public ModelSingleProperty(TModel initialValue) : base(initialValue)
		{ }

		public override TModel Value
		{
			get => base.Value;
			set
			{
				if (ValueStream.Value != null)
				{
					ValueStream.Value.Disposing -= OnValueDisposing;
				}

				base.Value = value;
				if (ValueStream.Value != null)
				{
					ValueStream.Value.Disposing += OnValueDisposing;
				}
			}
		}

		private void OnValueDisposing(IModel disposed)
		{
			if (disposed is IModelCleanup model)
			{
				model.AddCleanupHandler(CleanupRoutine, CleanupPriority.High);
			}
		}

		private void CleanupRoutine(IModel _)
		{
			Value = default;
		}
	}
}