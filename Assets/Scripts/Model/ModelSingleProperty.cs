namespace pindwin.umvr.Model
{
	public class ModelSingleProperty<TModel> : SingleProperty<TModel>
		where TModel : class, IModel
	{
		public ModelSingleProperty(string label) : base(label)
		{ }

		public ModelSingleProperty(string label, TModel initialValue) : base(label, initialValue)
		{ }

		public override TModel Value
		{
			get => base.Value;
			set
			{
				if (base.Value == value)
				{
					return;
				}
				
				if (base.Value != null)
				{
					base.Value.Disposing -= OnValueDisposing;
				}

				base.Value = value;
				if (base.Value != null)
				{
					base.Value.Disposing += OnValueDisposing;
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