namespace pindwin.umvr.Model
{
	public interface IValueContainer<TValueType>
	{
		TValueType Value { get; set; }
	}
}