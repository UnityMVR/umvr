namespace pindwin.umvr.Model
{
	public static class ModelExtensions
	{
		public static bool IsValid(this IModel model) => model.Id != Id.UNKNOWN && model.Id != Id.DEFAULT;
	}
}