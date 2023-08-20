// ReSharper disable once CheckNamespace
namespace Narcolepsy.Platform.Extensions;

using GraphQL.Context;
using Requests;

public static class RequestManagerExtensions {
    public static void ConfigureGraphQL(this IRequestManager manager, Action<IGraphQLViewConfiguration> buildDelegate) =>
        manager.Configure("GraphQL", buildDelegate);
}