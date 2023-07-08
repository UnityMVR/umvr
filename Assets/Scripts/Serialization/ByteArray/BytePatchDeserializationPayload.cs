using System;
using System.IO;
using System.Text;
using pindwin.umvr.Model;

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

		public void Dispose()
		{
			_reader?.Dispose();
			_stream?.Dispose();
		}
	}
}