namespace Stas.Monitor.Presentations;

public interface IUiThreadInvoker
{
    Task InvokeOnUIThreadAsync(Action action);
}
