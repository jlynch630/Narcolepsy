namespace Narcolepsy.Platform.State;

public interface IReadOnlyState<TValue> {
	public TValue Value { get; }

	public event EventHandler<StateChangeEventArgs<TValue>>? ValueChanged;
}

public class StateChangeEventArgs<T> : EventArgs {
	public T Value { get; }

    public StateChangeEventArgs(T value) {
        this.Value = value;
    }
}