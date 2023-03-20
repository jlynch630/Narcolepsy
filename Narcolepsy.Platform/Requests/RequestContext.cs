namespace Narcolepsy.Platform.Requests; 
using Narcolepsy.Platform.Serialization;
using Narcolepsy.Platform.State;

public abstract class RequestContext<TSaveState> : IRequestContext {
    public MutableState<string> Name { get; } = new("");

    public abstract TSaveState Save();

    public void Save(IContextStore store) {
        store.Put(this.Save());
    }
}
