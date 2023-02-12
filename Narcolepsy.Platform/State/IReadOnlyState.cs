namespace Narcolepsy.Platform.State;

public interface IReadOnlyState<TValue> {
	public TValue Value { get; }

	public event EventHandler<TValue>? ValueChanged;
}