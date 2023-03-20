namespace Narcolepsy.App.Services;

using Narcolepsy.Platform.Collections;
using Narcolepsy.Platform.Requests;
using Narcolepsy.Platform.Serialization;
using Narcolepsy.Platform.State;
using System;
using System.Text.Json;
using System.Threading.Tasks;

internal class RequestStorage : IRequestStorage {
    private SerializationManager SerializationManager;
    private RequestManager RequestManager;

    public RequestStorage(SerializationManager serializationManager, RequestManager requestManager) {
        this.SerializationManager = serializationManager;
        this.RequestManager = requestManager;
    }

    public async Task<string> SaveRequestAsync(Request request, string fileId = null) {
        RequestSnapshot Snapshot = await this.SerializationManager.SerializeRequestAsync(request);

        // then save it somewhere!
        string FileId = fileId ?? Guid.NewGuid().ToString("N");
        string FilePath = RequestStorage.ResolvePathByFileId(FileId);

        await File.WriteAllBytesAsync(FilePath, Snapshot.Serialize());
        return FileId;
    }

    public async Task<Request> LoadRequestAsync(string fileId) {
        string FilePath = RequestStorage.ResolvePathByFileId(fileId);
        byte[] FileBytes = await File.ReadAllBytesAsync(FilePath);
        RequestSnapshot Snapshot = RequestSnapshot.Deserialize(FileBytes);

        return await this.RequestManager.CreateRequestAsync(Snapshot);
    }

    public async Task<Collection> LoadCollectionAsync(string collectionId) {
        try {
            string Path = RequestStorage.ResolvePathByCollectionId(collectionId);

            string Text = await File.ReadAllTextAsync(Path);

            return JsonSerializer.Deserialize<Collection>(Text);
        } catch (FileNotFoundException) {
            return null;
        }
    }

    public async Task<string> SaveCollectionAsync(Collection collection, string collectionId = null) {
        string CollectionId = collectionId ?? Guid.NewGuid().ToString("N");
        string Path = RequestStorage.ResolvePathByCollectionId(CollectionId);
        string Json = JsonSerializer.Serialize(collection);

        await File.WriteAllTextAsync(Path, Json);
        return CollectionId;
    }

    private static string ResolvePathByCollectionId(string collectionId) => Path.Combine(FileSystem.Current.AppDataDirectory, $"{collectionId}.collection");

    private static string ResolvePathByFileId(string fileId) => Path.Combine(FileSystem.Current.AppDataDirectory, $"{fileId}.req");
}
