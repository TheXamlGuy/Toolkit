﻿using Mediator;

namespace Toolkit.Framework.Foundation;

public class Mediator : IMediator
{
    private readonly IServiceProvider factory;

    public Mediator(IServiceProvider factory)
    {
        this.factory = factory;
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamCommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        throw new NotImplementedException();
    }

    public ValueTask Publish(object notification, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        dynamic? handler = factory.GetService(typeof(RequestClassHandlerWrapper<,>).MakeGenericType(request.GetType(), typeof(TResponse)));  
        if (handler is not null)
        {
            return handler.Handle((dynamic)request, cancellationToken);
        }

        return default;
    }

    public ValueTask<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        dynamic? handler = factory.GetService(typeof(CommandClassHandlerWrapper<,>).MakeGenericType(command.GetType(), typeof(TResponse)));
        if (handler is not null)
        {
            return handler.Handle((dynamic)command, cancellationToken);
        }

        return default;
    }

    public ValueTask<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        dynamic? handler = factory.GetService(typeof(QueryClassHandlerWrapper<,>).MakeGenericType(query.GetType(), typeof(TResponse)));
        if (handler is not null)
        {
            return handler.Handle((dynamic)query, cancellationToken);
        }

        return default;
    }

    public ValueTask<object?> Send(object message, CancellationToken cancellationToken = default)
    {
        if (message.GetType().GetInterface(typeof(IRequest<>).Name) is { } requestInterface)
        {
            if (requestInterface.GetGenericArguments() is { Length: 1 } arguments)
            {
                Type responseType = arguments[0];

                dynamic? handler = factory.GetService(typeof(RequestClassHandlerWrapper<,>).MakeGenericType(message.GetType(), responseType));
                if (handler is not null)
                {
                    return handler.Handle((dynamic)message, cancellationToken);
                }
            }
        }

        if (message.GetType().GetInterface(typeof(ICommand<>).Name) is { } commandInterface)
        {
            if (commandInterface.GetGenericArguments() is { Length: 1 } arguments)
            {
                Type responseType = arguments[0];

                dynamic? handler = factory.GetService(typeof(CommandClassHandlerWrapper<,>).MakeGenericType(message.GetType(), responseType));
                if (handler is not null)
                {
                    return handler.Handle((dynamic)message, cancellationToken);
                }
            }
        }

        if (message.GetType().GetInterface(typeof(IQuery<>).Name) is { } queryInterface)
        {
            if (queryInterface.GetGenericArguments() is { Length: 1 } arguments)
            {
                Type responseType = arguments[0];

                dynamic? handler = factory.GetService(typeof(QueryClassHandlerWrapper<,>).MakeGenericType(message.GetType(), responseType));
                if (handler is not null)
                {
                    return handler.Handle((dynamic)message, cancellationToken);
                }
            }
        }

        return default;
    }
}