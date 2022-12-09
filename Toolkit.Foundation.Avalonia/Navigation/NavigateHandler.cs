﻿using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Mediator;

namespace Toolkit.Foundation.Avalonia
{
    public class NavigateHandler : IRequestHandler<Navigate>
    {
        private readonly INavigationRouteDescriptorCollection descriptors;
        private readonly IMediator mediator;
        private readonly INamedDataTemplateFactory namedDataTemplateFactory;
        private readonly INamedTemplateFactory namedTemplateFactory;
        private readonly ITemplateFactory templateFactory;
        private readonly ITypedDataTemplateFactory typedDataTemplateFactory;

        public NavigateHandler(IMediator mediator,
            ITemplateFactory templateFactory,
            INamedTemplateFactory namedTemplateFactory,
            INamedDataTemplateFactory namedDataTemplateFactory,
            ITypedDataTemplateFactory typedDataTemplateFactory,
            INavigationRouteDescriptorCollection descriptors)
        {
            this.mediator = mediator;
            this.templateFactory = templateFactory;
            this.namedTemplateFactory = namedTemplateFactory;
            this.namedDataTemplateFactory = namedDataTemplateFactory;
            this.typedDataTemplateFactory = typedDataTemplateFactory;
            this.descriptors = descriptors;
        }

        public async ValueTask<Unit> Handle(Navigate request, CancellationToken cancellationToken)
        {
            object? content = null;
            object? template = null;

            Dictionary<string, object> keyedParameters = new();
            List<object> parameters = new();

            foreach (object? parameter in request.Parameters)
            {
                if (parameter is not null)
                {
                    if (parameter is KeyValuePair<string, object> keyed)
                    {
                        keyedParameters.Add(keyed.Key, keyed.Value);
                    }
                    else
                    {
                        parameters.Add(parameter);
                    }
                }
            }

            if (request.Name is { Length: > 0 } name)
            {
                content = namedDataTemplateFactory.Create(name, parameters.ToArray());
                template = namedTemplateFactory.Create(name);
            }

            if (request.Type is Type type)
            {
                content = typedDataTemplateFactory.Create(type, parameters.ToArray());
                template = templateFactory.Create(content);
            }

            if (template is not null)
            {
                object? route = null;
                if (descriptors.FirstOrDefault(x => request.Route is string { } name && name == x.Name) is NavigationRouteDescriptor descriptor)
                {
                    route = descriptor.Route;
                }
                else
                {
                    route = template;
                }

                if (route is Frame frame)
                {
                    await mediator.Send(new FrameNavigation(frame, content, template, keyedParameters));
                }

                if (route is ContentDialog dialog)
                {
                    await mediator.Send(new ContentDialogNavigation(dialog, content, template, keyedParameters));
                }

                if (route is ContentControl contentControl)
                {
                    await mediator.Send(new ContentControlNavigation(contentControl, content, template, keyedParameters));
                }
            }
            else
            {
                if (descriptors.FirstOrDefault(x => request.Route is string { } name && name == x.Name) is NavigationRouteDescriptor descriptor)
                {
                    if (descriptor.Route is ContentControl contentControl)
                    {
                        contentControl.Content = null;
                    }
                }
            }

            return default;
        }
    }
}