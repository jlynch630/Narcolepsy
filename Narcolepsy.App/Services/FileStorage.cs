namespace Narcolepsy.App.Services;

using System.Text.Json;
using Collections;
using Platform.Logging;
using Platform.Requests;
using Platform.Serialization;
using Plugins;
using Plugins.Requests;

internal class FileStorage : IStorage {
    private readonly RequestManager RequestManager;
    private readonly SerializationManager SerializationManager;

    public FileStorage(SerializationManager serializationManager, RequestManager requestManager) {
        this.SerializationManager = serializationManager;
        this.RequestManager = requestManager;
        Logger.Debug("Using FileStorage storage backend. Path: {Path}", FileStorage.ResolveMasterPath());
    }

    public async Task<CollectionMetadata[]> GetCollectionsAsync() {
        try {
            string Path = FileStorage.ResolveMasterPath();
            string Text = await File.ReadAllTextAsync(Path);
            CollectionMetadata[] Out = JsonSerializer.Deserialize<CollectionMetadata[]>(Text);
			if (Out is not null) return Out;

			Logger.Warning("Failed to deserialize master collections list. Returning empty array");
		} catch (FileNotFoundException) {
            Logger.Verbose("Unable to load master collections list. File not found. Returning empty array");
        }

		return Array.Empty<CollectionMetadata>();
	}

	public async Task<Collection> LoadCollectionAsync(string collectionId) {
        try {
            string Path = FileStorage.ResolvePathByCollectionId(collectionId);

            string Text = await File.ReadAllTextAsync(Path);

            Logger.Verbose("Loading {Length} byte collection {Id} from {Path}", Text.Length, collectionId, Path);
            CollectionSnapshot Snapshot = JsonSerializer.Deserialize<CollectionSnapshot>(Text);

            // then just create the collection from the snapshot
            SavedRequest[] SavedRequests = await Task.WhenAll(Snapshot.FileIds.Select(this.LoadRequestAsync));
            List<SavedRequest> FilteredSavedRequests = SavedRequests.Where(r => r is not null).ToList();
            if (FilteredSavedRequests.Count < SavedRequests.Length)
                Logger.Warning(
                    "Unable to successfully load all requests from collection {Id}. Requested: {RequestCount}, Loaded: {ActualCount}",
                    collectionId, SavedRequests.Length, FilteredSavedRequests.Count);
            
            Logger.Verbose("Loaded collection {Id} with {Count} requests", collectionId, FilteredSavedRequests.Count);
            return new Collection(collectionId, Snapshot.Name, FilteredSavedRequests);
        }
        catch (FileNotFoundException e) {
            Logger.Warning(e, "Attempted to load nonexistent collection {CollectionId}", collectionId);
            return null;
        }
    }
	
    private async Task UpdateMasterRecord(Collection collection) {
        string Path = FileStorage.ResolveMasterPath();

        // better hope this doesnt happen in parallel
        CollectionMetadata[] Current = await this.GetCollectionsAsync();
        CollectionMetadata Metadata = new(collection.Id, collection.Name, DateTime.Now, collection.Requests.Count, new string[] {});
        int Index = Array.FindIndex(Current, x => x.Id == collection.Id);
        if (Index == -1) {
            Current = Current.Concat(Enumerable.Repeat(Metadata, 1)).ToArray();
        } else {
            Current[Index] = Metadata;
        }

        string AsJson = JsonSerializer.Serialize(Current);
        await File.WriteAllTextAsync(Path, AsJson);
    }

	public Task<Collection> DeleteCollectionAsync(string collectionId) {
        throw new NotImplementedException();
    }

    public async Task<SavedRequest> LoadRequestAsync(string id) {
        string FilePath = FileStorage.ResolvePathByFileId(id);
        byte[] FileBytes = await File.ReadAllBytesAsync(FilePath);
        try {
            Logger.Verbose("Loading {FileBytes} byte request {Id} from {Path}", FileBytes.Length, id, FilePath);
            RequestSnapshot Snapshot = RequestSnapshot.Deserialize(FileBytes);
            Request Req = await this.RequestManager.CreateRequestAsync(Snapshot);

            return new SavedRequest(id, Req);
        }
        catch (Exception e) {
            Logger.Warning(e, "Failed to deserialize request {RequestId}", id);
            return null;
        }
    }

    public async Task SaveCollectionAsync(Collection collection) {
        string Path = FileStorage.ResolvePathByCollectionId(collection.Id);
        string Json = JsonSerializer.Serialize(collection.ToSnapshot());

        // save every request
        await Task.WhenAll(collection.Requests.Select(this.SaveRequestAsync));

        await File.WriteAllTextAsync(Path, Json);
        await this.UpdateMasterRecord(collection);
        Logger.Verbose("Saved {FileBytes} byte collection {Id} to {Path}", Json.Length, collection.Id, Path);

    }

    public async Task SaveRequestAsync(SavedRequest request) {
        RequestSnapshot Snapshot = await this.SerializationManager.SerializeRequestAsync(request.Request);

        // save it somewhere!
        string FilePath = FileStorage.ResolvePathByFileId(request.Id);

        await File.WriteAllBytesAsync(FilePath, Snapshot.Serialize());
        Logger.Verbose("Saved request {Id} to {Path}", request.Id, FilePath);

    }

    public Task DeleteRequestAsync(string requestId) {
        string Path = FileStorage.ResolvePathByFileId(requestId);
        File.Delete(Path);
        Logger.Verbose("Deleted request {Id}", requestId);
        return Task.CompletedTask;
    }

    private static string ResolvePathByCollectionId(string collectionId) =>
        Path.Combine(FileSystem.Current.AppDataDirectory, $"{collectionId}.collection");

    private static string ResolvePathByFileId(string fileId) =>
        Path.Combine(FileSystem.Current.AppDataDirectory, $"{fileId}.req");

	private static string ResolveMasterPath() =>
		Path.Combine(FileSystem.Current.AppDataDirectory, $"master.cols");
}