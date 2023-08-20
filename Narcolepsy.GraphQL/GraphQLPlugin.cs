namespace Narcolepsy.GraphQL;

using Context;
using Narcolepsy.Platform.Extensions;
using Platform;

public class GraphQLPlugin : IPlugin {
    public string FullName => "GraphQL";

    public string Description => "Adds GraphQL query support to Narcolepsy";

    public PluginVersion Version => new(0, 0, 1);

    public Task InitializeAsync(NarcolepsyContext context) {
        context
            .Requests
            .RegisterType<IGraphQLRequestContext, GraphQLRequestContextSnapshot, GraphQLView, IGraphQLViewConfiguration>(
                "GraphQL", 
                (state) => new GraphQLRequestContext(),
                new GraphQLViewConfiguration())
            .ConfigureIcon("hub");

        context.Requests.ConfigureGraphQL(config => {
            //graphql
        });

        
        return Task.CompletedTask;
    }
}