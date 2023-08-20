namespace Narcolepsy.App.Services;

using Collections;

internal interface IStorage {
    public Task<CollectionMetadata[]> GetCollectionsAsync();

    public Task<Collection> LoadCollectionAsync(string collectionId);

    public Task<Collection> DeleteCollectionAsync(string collectionId);

    public Task<SavedRequest> LoadRequestAsync(string id);

    public Task SaveCollectionAsync(Collection collection);

    public Task SaveRequestAsync(SavedRequest request);

    public Task DeleteRequestAsync(string requestId);
}