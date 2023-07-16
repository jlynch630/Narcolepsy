namespace Narcolepsy.Core.Http.Body {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    internal class UrlEncodedFormBody : IHttpBody {
        public List<KeyValuePair<string, string>> FormInputs { get; set; } = new();

        public ValueTask WriteAsync(Stream target) {
            UrlEncoder Encoder = UrlEncoder.Create();
            string Body = String.Join("&", this.FormInputs.Select(f => $"{Encoder.Encode(f.Key)}={Encoder.Encode(f.Value)}"));

            return target.WriteAsync(Encoding.UTF8.GetBytes(Body));
        }
    }
}
