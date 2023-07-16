namespace Narcolepsy.Core.Http.Body;

using System.Text;

public class TextBody : IHttpBody {
    public string BodyContent { get; set; } = String.Empty;

    public ValueTask WriteAsync(Stream target) => target.WriteAsync(Encoding.UTF8.GetBytes(this.BodyContent));
}