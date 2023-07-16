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

        public async ValueTask WriteAsync(Stream target) {
            /*
             * Binary protocol Message, strict encoding, 12+ bytes:
               +--------+--------+--------+--------+--------+--------+--------+--------+--------+...+--------+--------+--------+--------+--------+
               |1vvvvvvv|vvvvvvvv|unused  |00000mmm| name length                       | name                | seq id                            |
               +--------+--------+--------+--------+--------+--------+--------+--------+--------+...+--------+--------+--------+--------+--------+
             */

            // version
            target.WriteByte(0b10000000);
            target.WriteByte(0b00000001);

            // unused
            target.WriteByte(0);

            // message type
            target.WriteByte(0b00000001);

            // name length
            IEnumerable<byte> NameLengthBytes = BitConverter.GetBytes(this.MethodName?.Length ?? 0);
            if (BitConverter.IsLittleEndian) NameLengthBytes = NameLengthBytes.Reverse();

            await target.WriteAsync(NameLengthBytes.ToArray());

            // name
            if (this.MethodName is not null)
                await target.WriteAsync(Encoding.UTF8.GetBytes(this.MethodName));

            // sequence id
            await target.WriteAsync(BitConverter.GetBytes(1));

            foreach (ThriftData ThriftData in this.Data) {
                ThriftData.WriteToStream(target);
            }

            target.WriteByte(0);
        }
    }
}
