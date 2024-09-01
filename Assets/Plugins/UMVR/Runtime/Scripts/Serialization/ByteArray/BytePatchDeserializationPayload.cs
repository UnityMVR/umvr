using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using pindwin.umvr.Model;
using pindwin.umvr.Repository;

namespace pindwin.umvr.Serialization.BytePatch
{
	public class BytePatchDeserializationPayload : IDisposable
	{
		private readonly BinaryReader _reader;
		private readonly MemoryStream _stream;

		public BytePatchDeserializationPayload(byte[] payload)
		{
			_stream = new MemoryStream(payload);
			_reader = new BinaryReader(_stream);
		}

		public float ReadFloat()
		{
			return _reader.ReadSingle();
		}

		public Id ReadId()
		{
			return new Id(_reader);
		}

		public int ReadInt()
		{
			return _reader.ReadInt32();
		}

		public string ReadString()
		{
			int length = _reader.ReadInt32();
			if (length == -1)
			{
				return null;
			}

			byte[] bytes = _reader.ReadBytes(length);
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		public void ReadCollectionBytes<TItem>(IList<TItem> array, IRepository<TItem> repository)
			where TItem : IModel
		{
			int length = _reader.ReadInt32();
			for (int i = 0; i < length; i++)
			{
				Id id = new Id(_reader);
				array.Add(repository.Get(id));
			}
		}
		
		public TItem ReadSinglePropertyBytes<TItem>(IRepository<TItem> repository)
			where TItem : IModel
		{
			Id id = new Id(_reader);
			return id == Id.UNKNOWN ? default : repository.Get(id);
		}

		public void Dispose()
		{
			_reader?.Dispose();
			_stream?.Dispose();
		}
	}
}