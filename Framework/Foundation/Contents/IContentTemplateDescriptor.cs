using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Framework.Foundation;

public interface IContentTemplateDescriptor
{
    Type ContentType { get; }

    ServiceLifetime Lifetime { get; }

    string? Name { get; }

    Type TemplateType { get; }
}