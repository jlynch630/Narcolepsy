namespace Narcolepsy.GraphQL.Context; 
using System;
using Narcolepsy.Platform.Serialization;
using Narcolepsy.Platform.State;

internal class GraphQLRequestContext : IGraphQLRequestContext {
    public MutableState<string> Name { get; } = new("");

    public void Save(IContextStore store) {
        store.Put<string>(null);
    }
}
