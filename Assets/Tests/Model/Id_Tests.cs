using System.IO;
using NUnit.Framework;
using pindwin.umvr.Model;

namespace Model
{
    public class Id_Tests
    {
        private Id[] _ids;

        [SetUp]
        public void SetUp()
        {
            _ids = new Id[10];
            for (int i = 0; i < _ids.Length; i++)
            {
                _ids[i] = Id.Next();
            }
        }

        [Test]
        public void each_instance_of_Id_is_unique()
        {
            for (int left = 0; left < _ids.Length; left++)
            {
                Assert.True(_ids[left].Equals(_ids[left]));
                for (int right = left + 1; right < _ids.Length; right++)
                {
                    Assert.True(_ids[left] != _ids[right]);
                    Assert.False(_ids[left] == _ids[right]);
                    Assert.False(_ids[left].Equals(_ids[right]));
                }
            }
        }

        [Test]
        public void deserialization_restores_Id_correctly()
        {
            for (int i = 0; i < _ids.Length; i++)
            {
                using var wms = new MemoryStream();
                using var writer = new BinaryWriter(wms);
                _ids[i].Write(writer);

                byte[] bytes = wms.ToArray();
                using var rms = new MemoryStream(bytes);
                using var reader = new BinaryReader(rms);
                
                var restored = new Id(reader);
                Assert.True(_ids[i] == restored);
            }
        }
    }
}
