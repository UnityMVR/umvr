namespace pindwin.umvr.Command
{
	public interface IConditionalCommand : ICommand
	{
		bool CanExecute();
	}

	public interface IConditionalCommand<in TParam> : ICommand<TParam>, IValidatable<TParam>
	{
		bool CanExecute();
	}
}