namespace Narcolepsy.Platform.State;

using Narcolepsy.Platform.Requests;
using System.Threading.Tasks;

public interface IRequestStorage {
    public Task<string> SaveRequestAsync(Request request, string? fileId = null);

    public Task<Request> LoadRequestAsync(string fileId);
}
