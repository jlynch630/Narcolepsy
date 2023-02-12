namespace Narcolepsy.Core.Http {
    using System.IO.Pipelines;

    public class HttpRequest {
		private readonly Pipe BodyPipe = new();

		public HttpRequest(string method, Uri uri) {
			this.Method = method;
			this.Uri = uri;
		}

		public string Method { get; }

		public Uri Uri { get; }

		public PipeWriter Body => this.BodyPipe.Writer;

		public Dictionary<string, string> Headers { get; } = new();

		private async Task<HttpRequestMessage> CreateRequestAsync() {
			await this.BodyPipe.Reader.CompleteAsync();
			HttpContent Content = new StreamContent(this.BodyPipe.Reader.AsStream());

			// do content headers first
			foreach ((string Name, string Value) in this.Headers.Where(
				         kvp => kvp.Key.StartsWith("Content-", StringComparison.OrdinalIgnoreCase))) {
				Content.Headers.Add(Name, Value);
			}

			// then other headers
			HttpRequestMessage Message = new()
				       {
					       Method = new HttpMethod(this.Method), RequestUri = this.Uri, Content = Content
				       };

			foreach ((string Name, string Value) in this.Headers)
				Message.Headers.Add(Name, Value);

			return Message;
		}
	}
}
