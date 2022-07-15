namespace Yarde.WindowSystem.WindowProvider
{
    internal interface IWindowProvider
    {
        T GetWindow<T>(WindowType windowName) where T : WindowBase;
    }

}
