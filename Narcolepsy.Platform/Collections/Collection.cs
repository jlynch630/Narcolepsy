namespace Narcolepsy.Platform.Collections;

using Narcolepsy.Platform.Requests;
using Narcolepsy.Platform.State;
using System.Collections;
using System.Text.Json.Serialization;

public class Collection {
    private class SavedRequest {
        public SavedRequest(string id) : this(id, null) { }

        public SavedRequest(string id, Request? request) {
            this.Id = id;
            this.Request = request;
        }

        public string Id { get; }

        public Request? Request { get; set; }
    }

    private List<SavedRequest> Requests = new();

    public Collection(string name) {
        this.Name = name;
    }

    [JsonConstructor]
    public Collection(string name, string[] fileIds) : this(name) {
        this.Requests = fileIds.Select(i => new SavedRequest(i)).ToList();
    }

    public string Name { get; }

    public string[] FileIds => this.Requests.Select(i => i.Id).ToArray();

    public async Task SaveRequestAsync(Request request, IRequestStorage storage) {
        // do we already have this request in the collection?
        SavedRequest? Existing = this.Requests.FirstOrDefault(r => r.Request == request);

        // either create a new id or use the existing
        string Id = await storage.SaveRequestAsync(request, Existing?.Id);

        if (Existing is null)
            this.Requests.Add(new SavedRequest(Id, request));
    }

    public async Task<Request[]> GetRequestsAsync(IRequestStorage storage) => await Task.WhenAll(this.Requests.Select(r => Collection.GetOrLoadRequestAsync(r, storage)));

    private static async Task<Request> GetOrLoadRequestAsync(SavedRequest request, IRequestStorage storage) => request.Request ??= await storage.LoadRequestAsync(request.Id);
}
