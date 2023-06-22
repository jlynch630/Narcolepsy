namespace Narcolepsy.Thrift {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal record ThriftData(ThriftDataType Type, int Index, string StringValue) {
        public void WriteToStream(Stream target) {
            sbyte FieldType = this.Type switch {
                ThriftDataType.Int8 => 3,
                ThriftDataType.Int16 => 6,
                ThriftDataType.Int32 => 8,
                ThriftDataType.Int64 => 10,
                ThriftDataType.String => 11,
                ThriftDataType.Binary => 11,
                ThriftDataType.Double => 4,
                ThriftDataType.Bool => 2,
                _ => throw new ArgumentOutOfRangeException()
            };

            byte[] FieldValue = this.Type switch {
                ThriftDataType.Int8 => new[] { Byte.Parse(this.StringValue) },
                ThriftDataType.Int16 => BitConverter.GetBytes(Int16.Parse(this.StringValue)),
                ThriftDataType.Int32 => BitConverter.GetBytes(Int32.Parse(this.StringValue)),
                ThriftDataType.Int64 => BitConverter.GetBytes(Int64.Parse(this.StringValue)),
                ThriftDataType.String => Encoding.UTF8.GetBytes(this.StringValue),
                //ThriftDataType.Binary => Convert.FromBase64String(this.StringValue),
                ThriftDataType.Double => BitConverter.GetBytes(BitConverter.DoubleToInt64Bits(Double.Parse(this.StringValue))),
                ThriftDataType.Bool => new[] { this.StringValue.Equals("true", StringComparison.OrdinalIgnoreCase) ? (byte)1 : (byte)0 },
                _ => throw new ArgumentOutOfRangeException()
            };

            byte[] IndexBytes = BitConverter.GetBytes((short)this.Index);

            if (BitConverter.IsLittleEndian && this.Type is not (ThriftDataType.String or ThriftDataType.Binary)) {
                Array.Reverse(FieldValue);
                Array.Reverse(IndexBytes);
            }

            target.WriteByte((byte)FieldType);
            target.Write(IndexBytes);
            target.Write(FieldValue);
        }
    }
}
