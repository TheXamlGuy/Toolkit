﻿using FluentAvalonia.UI.Controls;
using Mediator;
using Toolkit.Framework.Foundation;

namespace Toolkit.Framework.Avalonia;

public class ContentDialogNavigationHandler : IRequestHandler<ContentDialogNavigation, bool>
{
    public async ValueTask<bool> Handle(ContentDialogNavigation request, CancellationToken cancellationToken)
    {
        if (request.Template is ContentDialog contentDialog)
        {
            async void HandleButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            {
                ContentDialogButtonClickDeferral defferal = args.GetDeferral();
                if (sender.DataContext is INavigationConfirmation confirmation)
                {
                    if (!await confirmation.CanConfirm())
                    {
                        args.Cancel = true;
                    }
                }

                if (!args.Cancel)
                {
                    contentDialog.SecondaryButtonClick -= HandleButtonClick;
                    contentDialog.PrimaryButtonClick -= HandleButtonClick;
                    contentDialog.CloseButtonClick -= HandleButtonClick;
                }

                defferal.Complete();
            }

            contentDialog.SecondaryButtonClick += HandleButtonClick;
            contentDialog.PrimaryButtonClick += HandleButtonClick;
            contentDialog.CloseButtonClick += HandleButtonClick;

            contentDialog.DataContext = request.Content;
            await contentDialog.ShowAsync();
        }

        return await Task.FromResult(true);
    }
}