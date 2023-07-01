namespace Narcolepsy.App.Plugins.Requests;

using Platform.Requests;

internal class RequestDefinition {
    public RequestDefinition(string name, RequestFactory requestFactory) {
        this.Name = name;
        this.RequestFactory = requestFactory;
    }

    public string Icon { get; private set; }

    public string Name { get; }

    public RequestFactory RequestFactory { get; }

    internal class RequestDefinitionBuilder : IRequestDefinitionBuilder {
        private readonly RequestDefinition Definition;

        public RequestDefinitionBuilder(RequestDefinition def) => this.Definition = def;

        public IRequestDefinitionBuilder ConfigureIcon(string icon) {
            this.Definition.Icon = icon;
            return this;
        }
    }
}