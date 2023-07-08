using System.Collections.Generic;
using System.Text;

namespace GenerationParams
{
	public class Constructor : Method
	{
		private string _baseOrThis
		{
			get
			{
				if (BaseConstructor == null)
				{
					return string.Empty;
				}
				
				return BaseConstructor.Type == Type ? "this" : "base";
			}	
		}
		
		public Constructor BaseConstructor { get; set; }
		
		public Constructor(string type, IEnumerable<string> descriptors = null, Constructor baseConstructor = null)
			: base(type, descriptors ?? new string[]{})
		{
			BaseConstructor = baseConstructor;
		}

		public string ToSignatureString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"{Descriptors.ToCollectionString()} {Type}({Params.ToMethodSignatureString()})");
			if (BaseConstructor != null)
			{
				sb.Append($" : {_baseOrThis}({BaseConstructor.Params.ToParameterNamesString()})");
			}
			return sb.ToString();
		}
	}
}