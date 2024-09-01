using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;
using static pindwin.development.Assertions;

namespace pindwin.umvr.Serialization.BytePatch
{
	public class BytePatchSerializationPayload : IDisposable
	{
		private readonly BinaryWriter _writer;
		private readonly MemoryStream _stream;
		private readonly byte[] _buffer;

		public int Position => (int)_stream.Position;
		public const int PATCH_SIZE_DEFAULT = 1024;

		public BytePatchSerializationPayload(int bufferSize = PATCH_SIZE_DEFAULT) : this(new byte[bufferSize])
		{ }

		public BytePatchSerializationPayload(byte[] payload)
		{
			_buffer = payload;
			_stream = new MemoryStream(_buffer);
			_writer = new BinaryWriter(_stream);
		}
		
		public void Reset()
		{
			_stream.Position = 0;
		}

		public long CopyBufferAndReset(byte[] buffer)
		{
			if (buffer.Length <= _stream.Position)
			{
				throw new UMVRException($"Array out of bounds when calling " +
										$"{nameof(BytePatchSerializationPayload)}.{nameof(CopyBufferAndReset)}. " +
										$"Passed buffer {buffer.Length} long, needed {_stream.Position} long.");
			}
			
			Array.Copy(_buffer, buffer, _stream.Position);
			long position = _stream.Position;
			_stream.Position = 0;
			return position;
		}

		public void WriteId(Id id)
		{
			id.Write(_writer);
		}
		
		public void WriteInt(int arg)
		{
			_writer.Write(arg);
		}

		public void WriteFloat(float arg)
		{
			_writer.Write(arg);
		}

		public void WriteString(string arg)
		{
			if (arg == null)
			{
				_writer.Write(-1);
				return;
			}
			
			byte[] payload = Encoding.UTF8.GetBytes(arg);
			_writer.Write(payload.Length);
			_writer.Write(payload);
		}
		
		public void WriteCollectionBytes<TItem>(IList<TItem> array)
			where TItem : IModel
		{
			int length = array.Count;
			_writer.Write(length);
			for (var i = 1; i <= length; i++)
			{
				Id id = array[i - 1].Id;
				id.Write(_writer);
			}
		}

		public void WriteSinglePropertyBytes<TSingle>(TSingle model)
			where TSingle : IModel
		{
			if (model == null)
			{
				Id id = Id.UNKNOWN;
				id.Write(_writer);
				return;
			}
			model.Id.Write(_writer);
		}

		public void Dispose()
		{
			_writer?.Dispose();
			_stream?.Dispose();
		}
	}
}