using Avalonia.Threading;

namespace Stas.Monitor.Presentations;

public class UiThreadInvoker : IUiThreadInvoker
{
    public async Task InvokeOnUIThreadAsync(Action action)
    {
        await Dispatcher.UIThread.InvokeAsync(action);
    }
}
