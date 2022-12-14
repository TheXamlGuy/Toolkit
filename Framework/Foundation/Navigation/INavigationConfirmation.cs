namespace Toolkit.Framework.Foundation;

public interface INavigationConfirmation
{
    ValueTask<bool> CanConfirm();
}

public interface INavigated
{
    ValueTask Navigated();
}