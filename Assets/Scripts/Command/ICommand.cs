namespace pindwin.umvr.Command
{
	public interface ICommand
	{
		void Execute();
	}
	
	public interface ICommand<in TParam>
	{
		void Execute(TParam param);
	}
}