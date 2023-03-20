namespace Narcolepsy.Platform.Serialization {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISerializationManager {
        void AddSerializer<T>(ISerializer serializer);
    }
}
