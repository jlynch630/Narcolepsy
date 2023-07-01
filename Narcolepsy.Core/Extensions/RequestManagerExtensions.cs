// ReSharper disable once CheckNamespace

namespace Narcolepsy.Platform.Extensions;

using Core.ViewConfig;
using Requests;

public static class RequestManagerExtensions {
    public static void ConfigureHttp(this IRequestManager manager, Action<IHttpViewConfiguration> buildDelegate) =>
        manager.Configure("HTTP", buildDelegate);
}