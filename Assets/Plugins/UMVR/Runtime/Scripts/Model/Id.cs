using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;

namespace pindwin.umvr.Model
{
	[Serializable]
	public struct Id : IEquatable<Id>
	{
		[SerializeField] private int _timestamp;
		[SerializeField] private int _id;

		public static readonly Id DEFAULT = new Id(0, 0);
		public static readonly Id UNKNOWN = new Id(0, -1);
		public const int SIZEOF = sizeof(int) + sizeof(int);

		private static int incrementalId;
		private static int _appRunSignature;
		
		static Id()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += change =>
			{
				if (change == UnityEditor.PlayModeStateChange.EnteredPlayMode)
				{
					RefreshSignature();
				}
			};
			RefreshSignature();

			void RefreshSignature()
			{
				_appRunSignature = GetUnixTimestamp(Process.GetCurrentProcess().StartTime +
													TimeSpan.FromSeconds(UnityEditor.EditorApplication.timeSinceStartup));
			}
#else
			_appRunSignature = GetUnixTimestamp(Process.GetCurrentProcess().StartTime);
#endif
		}

		private Id(int timestamp, int id)
		{
			_timestamp = timestamp;
			_id = id;
		}

		public Id(BinaryReader reader)
		{
			_timestamp = reader.ReadInt32();
			_id = reader.ReadInt32();
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(_timestamp);
			writer.Write(_id);
		}
		
		public static Id Next()
		{
			return new Id(_appRunSignature, Interlocked.Increment(ref incrementalId));
		}

		public bool Equals(Id other)
		{
			return _timestamp == other._timestamp && _id == other._id;
		}

		public override bool Equals(object obj)
		{
			return obj is Id other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_timestamp.GetHashCode() * 397) ^ _id;
			}
		}

		public static bool operator ==(Id left, Id right)
		{
			return left._id == right._id && left._timestamp == right._timestamp;
		}

		public static bool operator !=(Id left, Id right)
		{
			return !(left == right);
		}

		public override string ToString()
		{
			return $"{_id}@{_timestamp}";
		}

		public static Id Parse(string param)
		{
			string[] parts = param.Split('@');
			return new Id(int.Parse(parts[1]), int.Parse(parts[0]));
		}
		
		private static int GetUnixTimestamp(DateTime stampTime)
		{
			return (int) (stampTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		}
	}
}