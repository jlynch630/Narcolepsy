namespace Narcolepsy.Core.Http.Body;

internal class EmptyBody : IHttpBody {
    public ValueTask WriteAsync(Stream target) => ValueTask.CompletedTask;
}