namespace Narcolepsy.Core.Http;

using Platform.Requests;
using Platform.State;

public interface IHttpRequestContext : IRequestContext {
	public MutableState<string> Url { get; }

	public MutableState<string> Method { get; }

	public MutableState<IHttpBody> Body { get; }

	public MutableListState<HttpHeader> Headers { get; }

	public IReadOnlyState<HttpResponse?> Response { get; }

	public Task Execute(CancellationToken token);
}