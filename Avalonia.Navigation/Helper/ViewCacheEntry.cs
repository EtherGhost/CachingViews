using Avalonia.Controls;

namespace Avalonia.Navigation.Helper;

public struct ViewCacheEntry
{
    public Control View { get; }
    public string ViewModelName { get; }
    public int Id { get; }

    public ViewCacheEntry(Control view, string viewModelName, int id)
    {
        View = view;
        ViewModelName = viewModelName;
        Id = id;
    }
}