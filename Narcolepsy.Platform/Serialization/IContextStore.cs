namespace Narcolepsy.Platform.Serialization;

public interface IContextStore {
    void Put<T>(T saveState);
}
