namespace GenerationParams
{
	public abstract class Member : Token
	{
		public new string Name { get => _nameToken.Name; set => _nameToken = new Token(value); }
		public string Type { get => _typeToken.Name; set => _typeToken = new Token(value); }

		private Token _nameToken;
		private Token _typeToken;
	}
}