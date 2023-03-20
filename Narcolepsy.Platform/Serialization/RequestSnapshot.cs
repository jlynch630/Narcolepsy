namespace Narcolepsy.Platform.Serialization {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public record RequestSnapshot(string RequestType, byte[] SaveState) {
        public RequestSnapshot(string requestType, ReadOnlySpan<byte> saveState) : this(requestType, saveState.ToArray()) { }

        public byte[] Serialize() {
            // very simple serialization: first byte is request type length, then request type, then save state
            byte[] RequestTypeBytes = Encoding.UTF8.GetBytes(this.RequestType);

            if (RequestTypeBytes.Length > 255) throw new NotImplementedException("todo");
            byte RequestTypeLength = (byte)RequestTypeBytes.Length;

            byte[] Serialized = new byte[1 + RequestTypeLength + SaveState.Length];

            // copy length
            Serialized[0] = RequestTypeLength;

            // request type
            Array.Copy(RequestTypeBytes, 0, Serialized, 1, RequestTypeLength);

            // save state
            SaveState.CopyTo(Serialized.AsSpan()[(1 + RequestTypeLength)..]);
            return Serialized;
        }

        public static RequestSnapshot Deserialize(byte[] serialized) {
            byte RequestTypeLength = serialized[0];
            string RequestType = Encoding.UTF8.GetString(serialized, 1, RequestTypeLength);
            return new RequestSnapshot(RequestType, serialized[(RequestTypeLength + 1)..]);
        }
    }
}
