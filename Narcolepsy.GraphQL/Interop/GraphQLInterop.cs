namespace Narcolepsy.GraphQL.Interop; 
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

internal class GraphQLInterop {
    private readonly Lazy<Task<IJSObjectReference>> ModuleTask;

    public GraphQLInterop(IJSRuntime jsRuntime) {
        this.ModuleTask = new Lazy<Task<IJSObjectReference>>(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Narcolepsy.GraphQL/script/index.js").AsTask());
    }

    public async Task InitializeAsync() {
        IJSObjectReference Module = await this.ModuleTask.Value;
        await Module.InvokeVoidAsync("initializeGraphQL");
    }

    public async Task SetSchemaAsync(string content) {
        IJSObjectReference Module = await this.ModuleTask.Value;
        await Module.InvokeVoidAsync("setSchema", content);
    }
}
