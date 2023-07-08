using pindwin.umvr.Model;

namespace pindwin.umvr.example
{
	public partial class FooModel
	{
		private SingleProperty<System.String> _text;
		public System.String Text
		{
			get => _text.Value;
			set
			{
				_text.Value = value;
			}
		}

		public System.String Text2
		{
			get; private set;
		}

	}
}