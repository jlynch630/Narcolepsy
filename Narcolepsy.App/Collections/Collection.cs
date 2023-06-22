namespace Narcolepsy.App.Collections;

internal record Collection(string Id, string Name, List<SavedRequest> Requests) {
    public Collection(string Name) : this(Guid.NewGuid().ToString("N"), Name, new List<SavedRequest>()) { }
}