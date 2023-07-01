namespace Narcolepsy.App.Collections;

using Narcolepsy.Platform.State;
using Platform.Requests;

public class Collection {
    private readonly List<SavedRequest> RequestList;

    public Collection(string name) : this(Guid.NewGuid().ToString("N"), name, new List<SavedRequest>()) { }

    public Collection(string id, string name, List<SavedRequest> requests) {
        this.Id = id;
        this.Name = name;
        this.RequestList = requests;
    }

    public string Id { get; }

    public string Name { get; }

    public MutableState<SavedRequest> ActiveRequest { get; } = new(null);

    public IReadOnlyList<SavedRequest> Requests => this.RequestList;

    public event EventHandler<CollectionUpdatedEventArgs> RequestAdded;

    public event EventHandler<CollectionUpdatedEventArgs> RequestRemoved;

    public void AddRequest(Request request) => this.AddRequest(new SavedRequest(request));

    public void AddRequest(SavedRequest request) {
        this.RequestList.Add(request);
        this.RequestAdded?.Invoke(this, new CollectionUpdatedEventArgs(request));
    }

    public void RemoveRequest(SavedRequest request) {
        this.RequestList.Remove(request);
        this.RequestRemoved?.Invoke(this, new CollectionUpdatedEventArgs(request));
    }

    internal CollectionSnapshot ToSnapshot() => new(this.Name, this.Requests.Select(f => f.Id).ToArray());
}