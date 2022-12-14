using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAvalonia(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<INavigationRouteDescriptorCollection, NavigationRouteDescriptorCollection>();

        serviceCollection.AddHandler<ContentHandler>();
        serviceCollection.AddHandler<NavigationRouteHandler>();
        serviceCollection.AddHandler<NavigateHandler>();
        serviceCollection.AddHandler<FrameNavigationHandler>();
        serviceCollection.AddHandler<ContentDialogNavigationHandler>();
        serviceCollection.AddHandler<ContentControlNavigationHandler>();

        return serviceCollection;
    }
}