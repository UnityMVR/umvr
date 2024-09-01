using System.Collections;
using System.Collections.Generic;

namespace GenerationParams
{
	public class TokensCollection<TMember> : IEnumerable<TMember>
		where TMember : Token
	{
		protected List<TMember> Members = new List<TMember>();

		public IEnumerator<TMember> GetEnumerator()
		{
			return Members.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Members).GetEnumerator();
		}

		public void Add(TMember item)
		{
			Members.Add(item);
		}

		public void Clear()
		{
			Members.Clear();
		}

		public bool Remove(TMember item)
		{
			return Members.Remove(item);
		}

		public int Count => Members.Count;

		public TMember this[int index] => Members[index];

		public void AddRange(IEnumerable<TMember> members)
		{
			Members.AddRange(members);
		}
	}
}