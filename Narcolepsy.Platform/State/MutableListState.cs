namespace Narcolepsy.Platform.State;

using System.Collections.ObjectModel;
using System.Collections.Specialized;

public class MutableListState<TValue> : IReadOnlyState<IReadOnlyCollection<TValue>>, IDisposable {
	public ObservableCollection<TValue> MutableValue { get; }

	public MutableListState() => this.MutableValue = new ObservableCollection<TValue>();

	public MutableListState(IEnumerable<TValue> collection) {
		this.MutableValue =
			collection as ObservableCollection<TValue> ?? new ObservableCollection<TValue>(collection);
		this.MutableValue.CollectionChanged += this.CollectionValueChanged;
	}

	public void Dispose() {
		this.MutableValue.CollectionChanged -= this.CollectionValueChanged;
	}

    public IReadOnlyCollection<TValue> Value => this.MutableValue;

	public event EventHandler<IReadOnlyCollection<TValue>>? ValueChanged;

	private void CollectionValueChanged(object? sender, NotifyCollectionChangedEventArgs e) {
		this.ValueChanged?.Invoke(this, this.Value);
	}
}