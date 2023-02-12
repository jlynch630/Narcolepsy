namespace Narcolepsy.Platform.State;

using System.Collections.ObjectModel;
using System.Collections.Specialized;

public class MutableListState<TValue> : IReadOnlyState<IReadOnlyCollection<TValue>>, IDisposable {
	private readonly ObservableCollection<TValue> BackingValue;

	public MutableListState() => this.BackingValue = new ObservableCollection<TValue>();

	public MutableListState(IEnumerable<TValue> collection) {
		this.BackingValue =
			collection as ObservableCollection<TValue> ?? new ObservableCollection<TValue>(collection);
		this.BackingValue.CollectionChanged += this.CollectionValueChanged;
	}

	public void Dispose() {
		this.BackingValue.CollectionChanged -= this.CollectionValueChanged;
	}

	public IReadOnlyCollection<TValue> Value => this.BackingValue;

	public event EventHandler<IReadOnlyCollection<TValue>>? ValueChanged;

	private void CollectionValueChanged(object? sender, NotifyCollectionChangedEventArgs e) {
		this.ValueChanged?.Invoke(this, this.Value);
	}
}