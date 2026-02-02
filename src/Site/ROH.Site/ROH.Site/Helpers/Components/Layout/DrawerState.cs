namespace ROH.Site.Helpers.Components.Layout;

public class DrawerState
{
    public bool Open { get; private set; }

    public event Action? OnChange;

    public void Toggle()
    {
        Open = !Open;
        OnChange?.Invoke();
    }
}