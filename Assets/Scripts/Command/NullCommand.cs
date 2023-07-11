namespace pindwin.umvr.Command
{
	public sealed class NullCommand : ConditionalCommand
	{
		public override bool CanExecute()
		{
			return false;
		}

		protected override void ExecuteImpl()
		{
			// do nothing
		}
	}

	public sealed class NullCommand<TParam> : ConditionalCommand<TParam>
	{
		public override bool CanExecute()
		{
			return false;
		}

		protected override void ExecuteImpl(TParam _)
		{
			// do nothing
		}

		public override bool IsValid(TParam payload)
		{
			return false;
		}
	}
}