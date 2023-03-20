namespace Narcolepsy.Platform.Requests;

using Narcolepsy.Platform.Serialization;
using State;

public interface IRequestContext {
    MutableState<string> Name { get; }

    void Save(IContextStore store);
}