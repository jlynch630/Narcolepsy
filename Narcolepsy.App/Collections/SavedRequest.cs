namespace Narcolepsy.App.Collections;

using Platform.Requests;

public record SavedRequest(string Id, Request Request) {
    public SavedRequest(Request request) : this(Guid.NewGuid().ToString("N"), request) { }
}