namespace pindwin.umvr.Command
{
	public interface IValidatable<in TPayload>
	{
		bool IsValid(TPayload payload);
	}
}