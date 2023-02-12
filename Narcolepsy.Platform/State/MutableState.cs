namespace Narcolepsy.Platform.State;

public class MutableState<TValue> : IReadOnlyState<TValue> {
	private TValue BackingValue;

	public MutableState(TValue initialValue) => this.BackingValue = initialValue;

	public TValue Value {
		get => this.BackingValue;
		set {
			this.BackingValue = value;
			this.ValueChanged?.Invoke(this, value);
		}
	}

	public event EventHandler<TValue>? ValueChanged;
}