namespace Narcolepsy.App.Collections; 

public class CollectionUpdatedEventArgs : EventArgs {
    public CollectionUpdatedEventArgs(SavedRequest request) => this.Request = request;

    public SavedRequest Request { get; }
}