namespace Narcolepsy.App.Services;

using Platform.Serialization;

internal class DuplicateContextStore : IContextStore {
    public object SaveState { get; private set; }

    public void Put<T>(T saveState) => this.SaveState = saveState;
}