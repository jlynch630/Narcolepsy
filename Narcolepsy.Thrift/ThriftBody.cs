namespace Narcolepsy.Thrift {
    using Narcolepsy.Core.Http.Body;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ThriftBody : IHttpBody {
        public string? MethodName { get; set; }

        public IList<ThriftData> Data { get; set; } = new List<ThriftData>();

        public ValueTask<Stream> GetStreamAsync() {
            // very strange but whatever
            Stream MemoryStream = new MemoryStream();

            /*
             * Binary protocol Message, strict encoding, 12+ bytes:
               +--------+--------+--------+--------+--------+--------+--------+--------+--------+...+--------+--------+--------+--------+--------+
               |1vvvvvvv|vvvvvvvv|unused  |00000mmm| name length                       | name                | seq id                            |
               +--------+--------+--------+--------+--------+--------+--------+--------+--------+...+--------+--------+--------+--------+--------+
             */

            // version
            MemoryStream.WriteByte(0b10000000);
            MemoryStream.WriteByte(0b00000001);

            // unused
            MemoryStream.WriteByte(0);

            // message type
            MemoryStream.WriteByte(0b00000001);

            // name length
            IEnumerable<byte> NameLengthBytes = BitConverter.GetBytes(this.MethodName?.Length ?? 0);
            if (BitConverter.IsLittleEndian) NameLengthBytes = NameLengthBytes.Reverse();

            MemoryStream.Write(NameLengthBytes.ToArray());

            // name
            if (this.MethodName is not null)
                MemoryStream.Write(Encoding.UTF8.GetBytes(this.MethodName));

            // sequence id
            MemoryStream.Write(BitConverter.GetBytes(1));

            foreach (ThriftData ThriftData in this.Data) {
                ThriftData.WriteToStream(MemoryStream);
            }

            MemoryStream.WriteByte(0);

            MemoryStream.Seek(0, SeekOrigin.Begin);
            return ValueTask.FromResult(MemoryStream);
        }
    }
}
