namespace Narcolepsy.App.Services;

using System.Text.Json;
using Collections;
using Platform.Requests;
using Platform.Serialization;

internal class FileStorage : IStorage {
    private readonly RequestManager RequestManager;
    private readonly SerializationManager SerializationManager;

    public FileStorage(SerializationManager serializationManager, RequestManager requestManager) {
        this.SerializationManager = serializationManager;
        this.RequestManager = requestManager;
    }

    public async Task<Collection> LoadCollectionAsync(string collectionId) {
        try {
            string Path = FileStorage.ResolvePathByCollectionId(collectionId);

            string Text = await File.ReadAllTextAsync(Path);

            CollectionSnapshot Snapshot = JsonSerializer.Deserialize<CollectionSnapshot>(Text);

            // then just create the collection from the snapshot
            SavedRequest[] SavedRequests = await Task.WhenAll(Snapshot.FileIds.Select(this.LoadRequestAsync));

            return new Collection(collectionId, Snapshot.Name, SavedRequests.ToList());
        }
        catch (FileNotFoundException) {
            return null;
        }
    }

    public async Task<SavedRequest> LoadRequestAsync(string id) {
        string FilePath = FileStorage.ResolvePathByFileId(id);
        byte[] FileBytes = await File.ReadAllBytesAsync(FilePath);
        RequestSnapshot Snapshot = RequestSnapshot.Deserialize(FileBytes);

        Request Req = await this.RequestManager.CreateRequestAsync(Snapshot);
        return new SavedRequest(id, Req);
    }

    public async Task SaveCollectionAsync(Collection collection) {
        string Path = FileStorage.ResolvePathByCollectionId(collection.Id);
        string Json = JsonSerializer.Serialize(collection);

        await File.WriteAllTextAsync(Path, Json);
    }

    public async Task SaveRequestAsync(SavedRequest request) {
        RequestSnapshot Snapshot = await this.SerializationManager.SerializeRequestAsync(request.Request);

        // save it somewhere!
        string FilePath = FileStorage.ResolvePathByFileId(request.Id);

        await File.WriteAllBytesAsync(FilePath, Snapshot.Serialize());
    }

    private static string ResolvePathByCollectionId(string collectionId) =>
        Path.Combine(FileSystem.Current.AppDataDirectory, $"{collectionId}.collection");

    private static string ResolvePathByFileId(string fileId) =>
        Path.Combine(FileSystem.Current.AppDataDirectory, $"{fileId}.req");
}