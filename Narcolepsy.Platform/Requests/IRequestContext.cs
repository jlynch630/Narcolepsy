namespace Narcolepsy.Platform.Requests;

using Narcolepsy.Platform.Serialization;
using State;

public interface IRequestContext {
    MutableState<string> Name { get; }

    // we save it into a context store so we don't have to generically type IRequestContext
    void Save(IContextStore store);
}