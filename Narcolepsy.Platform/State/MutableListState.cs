namespace Narcolepsy.Platform.State;

using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

public class MutableListState<TValue> : IList<TValue>, IReadOnlyState<IReadOnlyCollection<TValue>>, IDisposable {
	private ObservableCollection<TValue> MutableValue { get; }

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

    public int Count => ((ICollection<TValue>)this.MutableValue).Count;

    public bool IsReadOnly => ((ICollection<TValue>)this.MutableValue).IsReadOnly;

    public TValue this[int index] { get => ((IList<TValue>)this.MutableValue)[index]; set => ((IList<TValue>)this.MutableValue)[index] = value; }

    public event EventHandler<StateChangeEventArgs<IReadOnlyCollection<TValue>>>? ValueChanged;

	private void CollectionValueChanged(object? sender, NotifyCollectionChangedEventArgs e) {
		this.ValueChanged?.Invoke(this, new StateChangeEventArgs<IReadOnlyCollection<TValue>>(this.Value));
	}

    public int IndexOf(TValue item) => ((IList<TValue>)this.MutableValue).IndexOf(item);
    public void Insert(int index, TValue item) => ((IList<TValue>)this.MutableValue).Insert(index, item);
    public void RemoveAt(int index) => ((IList<TValue>)this.MutableValue).RemoveAt(index);
    public void Add(TValue item) => ((ICollection<TValue>)this.MutableValue).Add(item);
    public void Clear() => ((ICollection<TValue>)this.MutableValue).Clear();
    public bool Contains(TValue item) => ((ICollection<TValue>)this.MutableValue).Contains(item);
    public void CopyTo(TValue[] array, int arrayIndex) => ((ICollection<TValue>)this.MutableValue).CopyTo(array, arrayIndex);
    public bool Remove(TValue item) => ((ICollection<TValue>)this.MutableValue).Remove(item);
    public IEnumerator<TValue> GetEnumerator() => ((IEnumerable<TValue>)this.MutableValue).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.MutableValue).GetEnumerator();
}