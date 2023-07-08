namespace pindwin.umvr.Command
{
	public abstract class ConditionalCommand : IConditionalCommand
	{
		public void Execute()
		{
			if (CanExecute())
			{
				ExecuteImpl();
			}
		}

		public abstract bool CanExecute();

		protected abstract void ExecuteImpl();
	}

	public abstract class ConditionalCommand<TParam> : IConditionalCommand<TParam>
	{
		public void Execute(TParam param)
		{
			if (CanExecute(param))
			{
				ExecuteImpl(param);
			}
		}

		public abstract bool CanExecute(TParam param);

		protected abstract void ExecuteImpl(TParam param);
	}
}
